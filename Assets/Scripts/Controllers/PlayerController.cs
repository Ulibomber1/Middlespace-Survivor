using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour, IsoPlayer.IPlayerActions
{
    public float acceleration;
    public float maxVelocity;
    public float haltingDrag;

    Vector3 moveResult;
    Quaternion rotationResult;
    Rigidbody playerRigidbody;
 // Player playerEntity;

    void MovePlayer()
    {
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
    }

    // FixedUpdate is called once per physics tick
    void FixedUpdate()
    {
        MovePlayer();
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
}
