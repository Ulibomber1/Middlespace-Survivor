using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : EntityController
{
    GameObject playerReference;
    Vector3 distanceFromPlayer;

    private void Awake()
    {
        playerReference = GameObject.FindGameObjectWithTag("Player");
        hitPoints = maxHitPoints;
    }

    // Update is called once per frame
    void Update()
    {
        distanceFromPlayer = transform.position - playerReference.transform.position;

        if (distanceFromPlayer.x > 50 || distanceFromPlayer.x < -50 ||
            distanceFromPlayer.z > 50 || distanceFromPlayer.z < -50)
            this.gameObject.SetActive(false);

        if (hitPoints <= 0)
            this.gameObject.SetActive(false);
    }
}
