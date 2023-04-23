using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState {IDLE, CHASING_PLAYER, ATTACKING, FALLING}

public class EnemyController : EntityController
{
    [SerializeField] private float healthDropChance;

    GameObject playerReference;
    Vector3 positionDifference;
    [Range(0.0f, 1.0f)] public float rotationScalar;

    Rigidbody rb;
    Animator anim;
    protected EnemyState enemyState;
    [SerializeField] private float attackValue, minimumAttackDistance;

    [SerializeField] GameObject XP;
    [SerializeField] GameObject HP;
    [SerializeField] GameObject credit;
    private GameObject ParentPool;

    private void Awake()
    {
        playerReference = GameObject.FindGameObjectWithTag("Player");
        rb = gameObject.GetComponent<Rigidbody>();
        anim = gameObject.GetComponent<Animator>();
        ParentPool = transform.parent.gameObject;
    }

    private void ChangeAnimationState(EnemyState state)
    {
        Debug.Log((int)state);
        anim.SetInteger("EnemyState", (int)state);
    }

    private void OnEnable()
    {
        rb.detectCollisions = true;
        hitPoints = maxHitPoints;
        enemyState = EnemyState.IDLE;
        ChangeAnimationState(enemyState);
    }

    protected override void MoveEntity(Vector3 directionToPlayer, float distanceFromPlayer)
    {
        if (distanceFromPlayer <= minimumAttackDistance)
        {
            enemyState = EnemyState.ATTACKING;
            ChangeAnimationState(enemyState);
            rb.drag = haltingDrag;
            // activate attack mode (?)
            return;
        }
        rb.drag = 0;

        enemyState = EnemyState.CHASING_PLAYER;
        ChangeAnimationState(enemyState);
        rb.AddForce(directionToPlayer * acceleration);
        transform.rotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);

        if (rb.velocity.sqrMagnitude > maxVelocity * maxVelocity) // Using sqrMagnitude for efficiency
        {
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (enemyState == EnemyState.FALLING)
        {
            return;
        }

        positionDifference = -(transform.position - playerReference.transform.position);
        float distanceFromPlayer = positionDifference.magnitude;
        Vector3 directionToPlayer = positionDifference.normalized;

        // Despawn Stuff
        if (distanceFromPlayer > 50.0f || hitPoints <= 0.0f)
        {

            enemyState = EnemyState.FALLING;
            ChangeAnimationState(enemyState);
            rb.detectCollisions = false;
            Invoke("OnDespawn", 1f);

            if (hitPoints <= 0)
            {
                Instantiate(XP, transform.position, gameObject.transform.rotation.normalized);
                Instantiate(credit, transform.position, gameObject.transform.rotation.normalized);

                if (Random.Range(1f, 100f) <= healthDropChance)
                {
                    Instantiate(HP, transform.position, gameObject.transform.rotation.normalized);
                }
            }

            return;
        }
        //end Despawn Stuff

        MoveEntity(directionToPlayer, distanceFromPlayer);
    }

    public void OnTakeDamage(float damage)
    {
        hitPoints -= damage;
    }

    private void OnCollisionStay(Collision collision)
    {
        // This is constant, but it need to be variable and from
        // the enemies not the player calling it upon itself
        if (collision.gameObject.CompareTag("Player"))
            collision.gameObject.GetComponent<PlayerController>().InflictDamage(1);
    }

    private void OnDespawn()
    {
        gameObject.SetActive(false);
    }

    public delegate void EnemyDisabledHandler(GameObject pool);
    public static event EnemyDisabledHandler OnEnemyDiabled;
    private void OnDisable()
    {
        OnEnemyDiabled?.Invoke(ParentPool);
    }
}
