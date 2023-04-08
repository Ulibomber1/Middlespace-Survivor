using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] protected float damage;
    [SerializeField] protected float speed;
    [SerializeField] protected float maxDespawn;
    [SerializeField] protected float countdown;
    [SerializeField] protected bool isPlaying = true;
    [SerializeField] GameObject bulletSpawn;
    protected Rigidbody bulletRigidBody;

    protected void GameStateChange()
    {
        switch (GameManager.Instance.gameState)
        {
            case GameState.PLAYING:
                if (!isPlaying)
                    isPlaying = true;
                    bulletRigidBody.AddForce(transform.forward * speed);
                break;
            case GameState.PAUSE_MENU:
            case GameState.LEVELED_UP:
            case GameState.BUYING_EQUIPMENT:
                if (isPlaying)
                    isPlaying = false;
                    bulletRigidBody.AddForce(-(transform.forward * speed));
                break;
            case GameState.MAIN_MENU:
                DestroyBullet();
                break;
            case GameState.GAME_OVER:
                isPlaying = false;
                break;
        }

    }

    protected void Start()
    {
        GameManager.Instance.OnStateChange += GameStateChange;
        countdown = maxDespawn;
    }

    protected void Awake()
    {
        bulletRigidBody = GetComponent<Rigidbody>();
    }

    protected void OnEnable()
    {
        bulletRigidBody.AddForce(transform.forward * speed);
    }

    protected void Update()
    {
        if (GameManager.Instance.gameState == GameState.PLAYING)
        {
            countdown -= Time.deltaTime;

            if (countdown <= 0)
            {
                DestroyBullet();
            }
        }
    }

    protected void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
