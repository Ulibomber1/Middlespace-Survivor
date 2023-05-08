using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : EntityController, IsoPlayer.IPlayerActions, IDamageable
{
    protected float regenMod = 1;

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

    private float magnifierLevel = 0;
    private float batteryLevel = 0;
    private float movementMod = 1.0f;

    public delegate void PlayerDeadHandler();
    public static event PlayerDeadHandler OnPlayerDead;

    public delegate void playerDataChangeHandler(float hitPoints, float maxHitPoints, double experience, double maxExperience);
    public static event playerDataChangeHandler OnPlayerDataChange;

    // IDamageable Implementations
    public void InflictDamage(float rawDamage)
    {
        float dmgPercent = Mathf.Clamp(1 - damageResistance, 0.01f, 1);
        hitPoints -= dmgPercent * rawDamage;
        if (hitPoints <= 0.0f)
        {
            // Broadcast PlayerDead event
            OnPlayerDead?.Invoke();
        }
        OnPlayerDataChange?.Invoke(hitPoints, maxHitPoints, experience, maxExperience);
    }
    public void Heal(float healMod)
    {
        hitPoints += healthRegenFactor * healMod;
        OnPlayerDataChange?.Invoke(hitPoints, maxHitPoints, experience, maxExperience);
    }
    // IDamageable Implementations end

    public delegate void LevelUpHandler(int newLevel);
    public static event LevelUpHandler OnLevelUp;
    private void XPHandler(double amount)
    {
        experience += amount;
        totalExperience += amount;
        if (experience >= maxExperience)
        {
            experience -= maxExperience;
            playerLevel++;
            maxExperience = nextLevelScale * (.2 * playerLevel + Mathf.Pow(1.06f, (float)playerLevel));
            OnLevelUp?.Invoke(playerLevel);
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

        playerRigidbody.AddForce(moveResult.normalized * acceleration * movementMod);
        if (playerRigidbody.velocity.sqrMagnitude >= maxVelocity * maxVelocity) // Using sqrMagnitude for efficiency
        {
            playerRigidbody.velocity = playerRigidbody.velocity.normalized * maxVelocity * movementMod;
        }
    }

    private void Awake()
    {
        MouseTargetController.OnMouseTargetAwake += SetMouseTargetReference;
        XPDropController.OnXPPickedUp += XPHandler;
        ItemDataUtility.OnDataChange += HandleSelectedItem;
        HPDropController.OnHPPickedUp += Heal;
        healthRegenFactor = 1;
    }

    private void HandleSelectedItem(string name, int newlevel)
    {
        switch (name)
        {
            case "Reinforced Glass":
                damageResistance = newlevel / 100;
                break;
            case "Battery":
                batteryLevel = newlevel;
                break;
            case "Magnifier":
                magnifierLevel = newlevel;
                break;
            case "Portable Plug":
                movementMod = 1 + newlevel/20;
                break;
        }  
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
        OnLevelUp?.Invoke(playerLevel);
        targetMouse = GameObject.Find("Mouse Target");
    }
    private void SetMouseTargetReference(GameObject target)
    {
        targetMouse = target;
    }

    // FixedUpdate is called once per physics tick
    void FixedUpdate()
    {
        if (GameManager.Instance.gameState == GameState.PLAYING)
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
            Heal(Time.deltaTime * regenMod);
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
        GameObject temp;
        temp = (GameObject)Instantiate(bullet, bulletSpawn.transform.position, gameObject.transform.rotation.normalized);
        temp.GetComponent<PlayerBulletController>().ModifyDamage(batteryLevel);
        temp.GetComponent<PlayerBulletController>().ModifySize(magnifierLevel);
        //Quaternion.Euler(0, Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"))
    }
}


/*For later reference, player rotation code snippet
 *public Transform "tower";
 *tower.localRotation = Quaternion.AngleAxis(CannonAngle, Vector3.up);
 *https://www.youtube.com/watch?v=gaDFNCRQr38 */