using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DropController : MonoBehaviour
{
    [SerializeField] protected float amount;
    [SerializeField] protected float distance;
    [SerializeField] protected float distanceMod;
    [SerializeField] protected float speed;
    [SerializeField] protected float speedMod;
    [SerializeField] protected float speedMax;
    [SerializeField] protected bool isPlaying = true;
    protected float max;

    protected GameObject playerReference;
    protected Vector3 positionDifference;
    protected Vector3 directionToPlayer;
    protected float distanceFromPlayer;
    protected Vector3 currentForce;
    protected Rigidbody rb;

    protected void GameStateChange()
    {
        switch (GameManager.Instance.gameState)
        {
            case GameState.PLAYING:
                if (!isPlaying)
                    isPlaying = true;
                rb.AddForce(currentForce * speed);
                break;
            case GameState.PAUSE_MENU:
            case GameState.LEVELED_UP:
            case GameState.BUYING_EQUIPMENT:
                if (isPlaying)
                    isPlaying = false;
                currentForce = transform.forward;
                rb.AddForce(-(currentForce * speed));
                break;
            case GameState.MAIN_MENU:
                Destroy(gameObject);
                break;
            case GameState.GAME_OVER:
                isPlaying = false;
                break;
        }

    }

    private void Awake()
    {
        playerReference = GameObject.FindGameObjectWithTag("Player");
        rb = gameObject.GetComponent<Rigidbody>();
        rb.drag = 0;
        max = speedMax * speedMod;
    }

    private void FixedUpdate()
    {
        positionDifference = -(transform.position - playerReference.transform.position);
        distanceFromPlayer = positionDifference.magnitude;
        directionToPlayer = positionDifference.normalized;

        if (distanceFromPlayer < distance * distanceMod)
        {
            rb.AddForce(directionToPlayer * speed * speedMod);

            if (rb.velocity.sqrMagnitude > max * max) // Using sqrMagnitude for efficiency
            {
                rb.velocity = rb.velocity.normalized * max;
            }
        }

        if (distanceFromPlayer > 10000)
        {
            Remove();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == playerReference)
        {
            BroadcastAmount();

            Remove();
        }
    }

    protected void Remove()
    {
        Destroy(gameObject);
    }

    protected virtual void BroadcastAmount() {}
}
