using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : EntityController, IsoPlayer.IPlayerActions, IDamageable
{
    [SerializeField] protected float regenMod;
    [SerializeField] protected float regenRate;

    Vector3 moveResult;
    Quaternion rotationResult;
    Rigidbody playerRigidbody;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject bulletSpawn;
    GameObject targetMouse;
    [SerializeField] [Range(0, 1)] float rotationDampValue;
    //Vector2 mousePos;
    // Player playerEntity;
    double totalExperience = 0;
    double experience = 0;
    double maxExperience = 4570.8;
    int playerLevel = 1;
    [Range(1,5000)] public double nextLevelScale;
    // IDamageable Implementations

    float IDamageable.hitPoints { get { return hitPoints; } set { hitPoints = value; } }
    public float damageResistance { get; }
    public float healthRegenFactor { get; }

    public delegate void PlayerDeadHandler();
    public static event PlayerDeadHandler OnPlayerDead;

    public delegate void playerDataChangeHandler(float hitPoints, float maxHitPoints, double experience, double maxExperience);
    public static event playerDataChangeHandler OnPlayerDataChange;

    public void InflictDamage(float rawDamage)
    {
        hitPoints -= (1 - damageResistance) * rawDamage;
        if (hitPoints <= 0.0f)
        {
            // Broadcast PlayerDead event
            OnPlayerDead?.Invoke();
        }
        OnPlayerDataChange?.Invoke(hitPoints, maxHitPoints, experience, maxExperience);
    }
    public void Heal(float healAmount)
    {
        hitPoints += healthRegenFactor * healAmount;
        OnPlayerDataChange?.Invoke(hitPoints, maxHitPoints, experience, maxExperience);
    }
    // IDamageable Implementations end
    private void XPHandler(double amount)
    {
        experience += amount;
        totalExperience += amount;
        if (experience >= maxExperience)
        {
            experience -= maxExperience;
            playerLevel++;
            maxExperience = nextLevelScale * (.2 * playerLevel + Mathf.Pow(1.06f, (float)playerLevel));
        }
        OnPlayerDataChange?.Invoke(hitPoints, maxHitPoints, experience, maxExperience);
    }
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

    private void Awake()
    {
        MouseTargetController.OnMouseTargetAwake += SetMouseTargetReference;
        XPDropController.OnXPPickedUp += XPHandler;
    }
    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        GameManager.Instance.OnStateChange += GameStateChange;
        hitPoints = maxHitPoints;
        shotCoodown = maxShotCooldown;
        bulletSpawn = GameObject.Find("BulletSpawn");
        OnPlayerDataChange?.Invoke(hitPoints, maxHitPoints, experience, maxExperience);
        targetMouse = GameObject.Find("Mouse Target");
    }
    private void SetMouseTargetReference(GameObject target)
    {
        targetMouse = target;
    }

    // FixedUpdate is called once per physics tick
    void FixedUpdate()
    {
        MoveEntity();
    }

    private Vector3 IsoVectorConvert(Vector3 vector)
    {
        Quaternion rotation = Quaternion.Euler(0, -45.0f, 0); // The desired rotation in euler angles
        Matrix4x4 isoMatrix = Matrix4x4.Rotate(rotation); // Create a rotation matrix
        Vector3 result = isoMatrix.MultiplyPoint3x4(vector); // Rotate the input vector
        return result;
    }

    // OnMove() is used by the Unity Input System. Here its behavior is defined:
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 readVector = context.ReadValue<Vector2>();
        Vector3 toConvert = new Vector3(readVector.x, 0, readVector.y);
        moveResult = IsoVectorConvert(toConvert);
        //Vector3 relative = (transform.position + moveResult) - transform.position;
        //rotationResult = Quaternion.LookRotation(relative, Vector3.up);
    }

    private void Update()
    {
        if (GameManager.Instance.gameState == GameState.PLAYING)
        {
            shotCoodown -= Time.deltaTime;

            if (shotCoodown <= 0)
            {
                Shoot();
                shotCoodown = maxShotCooldown;
            }
        }

        if (hitPoints < maxHitPoints && isPlaying)
        {
            hitPoints += Time.deltaTime * regenMod * regenRate;
            OnPlayerDataChange?.Invoke(hitPoints, maxHitPoints, experience, maxExperience);
        }
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
      
    void Shoot()
    {
        Instantiate(bullet, bulletSpawn.transform.position, gameObject.transform.rotation.normalized); //Quaternion.Euler(0, Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"))
    }
}


/*For later reference, player rotation code snippet
 *public Transform "tower";
 *tower.localRotation = Quaternion.AngleAxis(CannonAngle, Vector3.up);
 *https://www.youtube.com/watch?v=gaDFNCRQr38 */