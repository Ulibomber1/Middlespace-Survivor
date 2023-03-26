using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    float hitPoints { get; set; }
    float damageResistance { get; }
    float healthRegenFactor { get; }
    void InflictDamage(float rawDamage);
    void Heal(float healAmount);
}
