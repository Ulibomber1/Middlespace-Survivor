using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ProjectyleType {PLAYER1, PLAYER2, ENEMY1, ENEMY2}

public class BulletController : MonoBehaviour
{
    [SerializeField] protected float damage;
    [SerializeField] protected float speed;
    protected Rigidbody bulletRigidBody;

    protected void Awake()
    {
        bulletRigidBody = GetComponent<Rigidbody>();
    }

    protected void OnEnable()
    {
        bulletRigidBody.AddForce(transform.up * speed);
        Invoke("DestroyBullet", 5f);
    }

    protected void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
