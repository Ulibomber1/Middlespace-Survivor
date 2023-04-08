using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : EntityController
{
    GameObject playerReference;
    Vector3 distanceFromPlayer;

    Rigidbody rb;

    private void Awake()
    {
        playerReference = GameObject.FindGameObjectWithTag("Player");
        hitPoints = maxHitPoints;
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        distanceFromPlayer = transform.position - playerReference.transform.position;
        // Despawn Stuff
        if (distanceFromPlayer.x > 50 || distanceFromPlayer.x < -50 ||
            distanceFromPlayer.z > 50 || distanceFromPlayer.z < -50)
        {
            this.gameObject.SetActive(false);
            return;
        }
        if (hitPoints <= 0)
        {
            this.gameObject.SetActive(false);
            return;
        }
        //end Despawn Stuff

        if (distanceFromPlayer.x > 10 || distanceFromPlayer.x < -10 ||
            distanceFromPlayer.z > 10 || distanceFromPlayer.z < -10)
        {
            // addForce towards the player
            //rb.AddForce()
            return;
        }
    }
}
