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
    protected float max;

    protected GameObject playerReference;
    protected Vector3 positionDifference;
    protected Vector3 directionToPlayer;
    protected float distanceFromPlayer;
    protected Rigidbody rb;

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
