using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player2Controller : EntityController, IsoPlayer.IPlayerActions
{
    //References

    Rigidbody playerRigidbody;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject bulletSpawn;
    GameObject targetMouse;
    GameObject Player1Reference = null;


    //Internal Variables

    Vector3 moveResult;
    Quaternion rotationResult;
    [SerializeField] [Range(0, 1)] float rotationDampValue;
    [SerializeField] private int player2DespawnDistance;
    [SerializeField] float invincibleRespawnTime;
    float timeActive;


    //User defined objects

    //Delegates

    public delegate void PlayerJoinedHandler(GameObject player);
    public delegate void Player2DeadHandler();


    //Events

    public static event PlayerJoinedHandler OnPlayerJoined;
    public static event Player2DeadHandler OnPlayer2Dead;


    //Unity Methods

    private void Awake()
    {
        //MouseTargetController.OnMouseTargetAwake += SetMouseTargetReference;
        targetMouse = GameObject.Find("Mouse Target");
    }

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        GameManager.Instance.OnStateChange += GameStateChange;
        OnPlayerJoined?.Invoke(gameObject);
    }

    void OnEnable()
    {
        hitPoints = maxHitPoints;
        shotCoodown = maxShotCooldown;
        timeActive = 0;
    }

    void FixedUpdate()
    {
        MoveEntity();
    }

    private void Update()
    {
        if (Player1Reference != null && (Player1Reference.transform.position -
            gameObject.transform.position).magnitude > player2DespawnDistance)
            Despawn();

        if (GameManager.Instance.gameState == GameState.PLAYING)
        {
            if (timeActive < invincibleRespawnTime)
                timeActive += Time.deltaTime;

            shotCoodown -= Time.deltaTime;

            if (shotCoodown <= 0)
            {
                Shoot();
                shotCoodown = maxShotCooldown;
            }
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnStateChange -= GameStateChange;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (timeActive >= invincibleRespawnTime &&
            (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Enemy Bullet")))
        {
            Despawn();
        }
    }


    //User-defined methods

    override protected void MoveEntity()
    {
        if (!isPlaying)
            return;

        Vector3 targetDirection = targetMouse.transform.position - transform.position;
        targetDirection.y = transform.position.y;
        Quaternion rotation = Quaternion.LookRotation(targetDirection);
        rotation.z = 0;
        rotation.x = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationDampValue);

        if (moveResult.magnitude == 0.0f)
        {
            playerRigidbody.drag = haltingDrag;
            playerRigidbody.constraints = RigidbodyConstraints.FreezeRotationY | 
                                          RigidbodyConstraints.FreezeRotationX |
                                          RigidbodyConstraints.FreezeRotationZ;
            return;
        }
        playerRigidbody.drag = 0;
        playerRigidbody.constraints = RigidbodyConstraints.FreezeRotationX |
                                      RigidbodyConstraints.FreezeRotationZ;


        
        playerRigidbody.AddForce(moveResult.normalized * acceleration);
        if (playerRigidbody.velocity.sqrMagnitude >= maxVelocity * maxVelocity) // Using sqrMagnitude for efficiency
        {
            playerRigidbody.velocity = playerRigidbody.velocity.normalized * maxVelocity;
        }
    }

    private Vector3 IsoVectorConvert(Vector3 vector)
    {
        Quaternion rotation = Quaternion.Euler(0, -45.0f, 0); // The desired rotation in euler angles
        Matrix4x4 isoMatrix = Matrix4x4.Rotate(rotation); // Create a rotation matrix
        Vector3 result = isoMatrix.MultiplyPoint3x4(vector); // Rotate the input vector
        return result;
    }

    void Despawn()
    {
        OnPlayer2Dead?.Invoke();
        gameObject.SetActive(false);
    }

    void Shoot()
    {
        Instantiate(bullet, bulletSpawn.transform.position, gameObject.transform.rotation.normalized); //Quaternion.Euler(0, Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"))
    }

    public void SetPlayerOneReference(GameObject reference)
    {
        Player1Reference = reference;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 readVector = context.ReadValue<Vector2>();
        Vector3 toConvert = new Vector3(readVector.x, 0, readVector.y);
        moveResult = IsoVectorConvert(toConvert);
        Debug.Log("moveresult magnitude: " + moveResult.magnitude);
    }

    // Here to complete interface, no implementations for either

    public void OnLook(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();


    }

    public void OnDamage(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }
}