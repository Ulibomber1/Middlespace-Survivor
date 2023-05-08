using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void InflictDamage(float rawDamage);
    public void Heal(float healAmount);
}
