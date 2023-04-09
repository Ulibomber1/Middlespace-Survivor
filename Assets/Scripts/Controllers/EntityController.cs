using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    [SerializeField] protected float hitPoints;
    [SerializeField] protected float maxHitPoints;
    [SerializeField] protected float acceleration;
    [SerializeField] protected float maxVelocity;
    [SerializeField] protected float haltingDrag;
    [SerializeField] protected float maxShotCooldown;
    [SerializeField] protected float shotCoodown;
    [SerializeField] protected bool isPlaying = true;

    protected void GameStateChange()
    {
        switch (GameManager.Instance.gameState)
        {
            case GameState.PLAYING:
                if (!isPlaying)
                    isPlaying = true;
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

    virtual protected void MoveEntity() { }
    virtual protected void MoveEntity(Vector3 directionToTarget, float distanceFromTarget) { }
}
