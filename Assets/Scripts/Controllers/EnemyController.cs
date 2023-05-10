using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState {IDLE, CHASING_PLAYER, ATTACKING, FALLING} // behavior states, will affect animations later


public class EnemyController : EntityController
{
    private float healthDropChance = 0;
    private int magnetLevel = 0;

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
        ItemDataUtility.OnDataChange += NewItemLevel;
        playerReference = GameObject.FindGameObjectWithTag("Player");
        rb = gameObject.GetComponent<Rigidbody>();
        anim = gameObject.GetComponent<Animator>();
        ParentPool = transform.parent.gameObject;
    }

    private void NewItemLevel(string name, int newLevel)
    {
        switch (name)
        {
            case "Magnet":
                magnetLevel = newLevel;
                break;
            case "Medkit":
                healthDropChance = newLevel * 10;
                break;
        }
    }

    private void ChangeAnimationState(EnemyState state)
    {
        //Debug.Log((int)state);
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
        if (enemyState == EnemyState.FALLING || GameManager.Instance.gameState != GameState.PLAYING)
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
                SpawnDrops();
            }

            return;
        }
        //end Despawn Stuff

        MoveEntity(directionToPlayer, distanceFromPlayer);
    }

    private void SpawnDrops()
    {
        GameObject temp = (GameObject)
        Instantiate(XP, transform.position, gameObject.transform.rotation.normalized);
        temp.GetComponent<DropController>().ChangeDistanceMod(magnetLevel);

        temp = (GameObject)
        Instantiate(credit, transform.position, gameObject.transform.rotation.normalized);
        temp.GetComponent<DropController>().ChangeDistanceMod(magnetLevel);

        float tempDropChance = healthDropChance;
        while (tempDropChance > 100)
        {
            temp = (GameObject)
            Instantiate(HP, transform.position, gameObject.transform.rotation.normalized);
            temp.GetComponent<DropController>().ChangeDistanceMod(magnetLevel);
            tempDropChance -= 100;
        }
        if (Random.Range(1f, 100f) <= tempDropChance)
        {
            temp = (GameObject)
            Instantiate(HP, transform.position, gameObject.transform.rotation.normalized);
            temp.GetComponent<DropController>().ChangeDistanceMod(magnetLevel);
        }
    }

    public void OnTakeDamage(float damage)
    {
        hitPoints -= damage;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Despawn Volume"))
            OnDespawn();
    }

    private void OnCollisionStay(Collision collision)
    {
        // This is constant, but it need to be variable and from
        // the enemies not the player calling it upon itself
        if (collision.gameObject.CompareTag("Player") && GameManager.Instance.gameState == GameState.PLAYING)
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

    private void OnDestroy()
    {
        ItemDataUtility.OnDataChange -= NewItemLevel;
    }
}
