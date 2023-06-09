using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropController : MonoBehaviour
{
    //Public
    public void ChangeDistanceMod(float newMod)
    {
        distanceMod = 1 + newMod / 20;
    }

    //Private (Protected)
    protected GameObject playerReference;
    protected Rigidbody rb;

    [SerializeField] protected float amount;
    [SerializeField] protected float distance;
    [SerializeField] protected float distanceMod;
    [SerializeField] protected float speed;
    [SerializeField] protected float speedMod;
    [SerializeField] protected float speedMax;
    [SerializeField] protected bool isPlaying = true;
    protected float max;
    protected float distanceFromPlayer;
    protected Vector3 currentForce;
    protected Vector3 positionDifference;
    protected Vector3 directionToPlayer;

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

        if (distanceFromPlayer == 0)
        {
            BroadcastAmount();
            Remove();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == playerReference)
        {
            Invoke("Remove", .1f);
            BroadcastAmount();
        }
    }

    protected void GameStateChange()
    {
        switch (GameManager.Instance.gameState)
        {
            case GameState.PLAYING:
                if (!isPlaying)
                {
                    isPlaying = true;
                }
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
    protected void Remove()
    {
        Destroy(gameObject);
    }
    protected virtual void BroadcastAmount() {}
}
