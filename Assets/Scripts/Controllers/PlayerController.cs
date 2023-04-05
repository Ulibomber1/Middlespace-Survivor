using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : EntityController, IsoPlayer.IPlayerActions, IDamageable
{
    Vector3 moveResult;
    Quaternion rotationResult;
    Rigidbody playerRigidbody;
    // Player playerEntity;

    private bool isPlaying = true;

    // IDamageable Implementations
    float IDamageable.hitPoints { get { return hitPoints; } set { hitPoints = value; } }
    public float damageResistance { get; }
    public float healthRegenFactor { get; }
    public delegate void PlayerDeadHandler();
    public static event PlayerDeadHandler OnPlayerDead;
    public void InflictDamage(float rawDamage)
    {
        hitPoints -= (1 - damageResistance) * rawDamage;
        if (hitPoints <= 0.0f)
        {
            // Broadcast PlayerDead event
            OnPlayerDead?.Invoke();
        }
    }
    public void Heal(float healAmount)
    {
        hitPoints += healthRegenFactor * healAmount;
    }
    // IDamageable Implementations end

    private void GameStateChange()
    {
        switch (GameManager.Instance.gameState)
        {
            case GameState.PLAYING:
                if (!isPlaying)
                    isPlaying =  true;
                break;
            case GameState.PAUSE_MENU:
            case GameState.LEVELED_UP:
            case GameState.BUYING_EQUIPMENT:
                if (isPlaying)
                    isPlaying = false;
                break;
            case GameState.MAIN_MENU:
                Destroy(gameObject);
                break;
            case GameState.GAME_OVER:
                isPlaying = false;
                break;
        }
           
    }

    override protected void MoveEntity()
    {
        if (!isPlaying)
            return;

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
        playerRigidbody.velocity += moveResult * acceleration;
        transform.rotation = rotationResult;
        if (playerRigidbody.velocity.sqrMagnitude > maxVelocity * maxVelocity) // Using sqrMagnitude for efficiency
        {
            playerRigidbody.velocity = playerRigidbody.velocity.normalized * maxVelocity;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        GameManager.Instance.OnStateChange += GameStateChange;
        hitPoints = maxHitPoints;
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
        Vector3 relative = (transform.position + moveResult) - transform.position;
        rotationResult = Quaternion.LookRotation(relative, Vector3.up);
    }

    // Here to complete interface, no implementations for either
    public void OnLook(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }
    public void OnFire(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnDamage(InputAction.CallbackContext context)
    {
        Debug.Log("OnDamage reached!");
        InflictDamage(90);
    }
       
}
