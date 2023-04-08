using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour, IDamageable
{
    [SerializeField] protected float hitPoints;
    [SerializeField] protected float maxHitPoints;
    [SerializeField] protected float acceleration;
    [SerializeField] protected float maxVelocity;
    [SerializeField] protected float haltingDrag;
    [SerializeField] protected float maxShotCooldown;
    [SerializeField] protected float shotCoodown;
    [SerializeField] protected bool isPlaying = true;

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
}
