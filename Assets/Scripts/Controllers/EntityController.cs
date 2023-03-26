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

    virtual protected void MoveEntity() { }
}
