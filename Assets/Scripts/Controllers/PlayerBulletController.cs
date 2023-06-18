using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletController : BulletController
{
    //References

    //Internal Variables

    //User defined objects

    //Delegates

    //Events

    //Unity Methods

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyController>().OnTakeDamage(damage * damageMod);
            DestroyBullet();
        }
    }


    //User-defined methods

    public void ModifyDamage(float newDamageLevel)
    {
        damageMod = 1 + (newDamageLevel)/10;
    }

    public void ModifySize(float newScale)
    {
        sizeMod = 1 + (newScale)/20;
        transform.localScale *= sizeMod;
    }
}
