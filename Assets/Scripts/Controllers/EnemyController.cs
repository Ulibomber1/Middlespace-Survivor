using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState {IDLE, CHASING_PLAYER, ATTACKING}

public class EnemyController : EntityController
{
    GameObject playerReference;
    Vector3 positionDifference;

    Rigidbody rb;
    protected EnemyState enemyState;
    [SerializeField] private float attackValue;

    private void Awake()
    {
        playerReference = GameObject.FindGameObjectWithTag("Player");
        hitPoints = maxHitPoints;
        rb = gameObject.GetComponent<Rigidbody>();
        enemyState = EnemyState.IDLE;
    }

    protected override void MoveEntity(Vector3 directionToPlayer, float distanceFromPlayer)
    {
        if (distanceFromPlayer < 0.1f)
        {
            enemyState = EnemyState.ATTACKING;
            rb.drag = haltingDrag;
            // activate attack mode (?)
            return;
        }
        rb.drag = 0;

        enemyState = EnemyState.CHASING_PLAYER;
        rb.AddForce(directionToPlayer * acceleration);
        if (rb.velocity.sqrMagnitude > maxVelocity * maxVelocity) // Using sqrMagnitude for efficiency
        {
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }
    }

    // Update is called once per frame
    void Update()
    {
        positionDifference = -(transform.position - playerReference.transform.position);
        float distanceFromPlayer = positionDifference.magnitude;
        Vector3 directionToPlayer = positionDifference.normalized;

        // Despawn Stuff
        if (distanceFromPlayer > 50.0f || hitPoints <= 0.0f)
        {
            enemyState = EnemyState.IDLE;
            this.gameObject.SetActive(false);
            return;
        }
        //end Despawn Stuff

        MoveEntity(directionToPlayer, distanceFromPlayer);
    }

    public void OnTakeDamage(float damage)
    {
        hitPoints -= damage;
    }
}
