using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZoneController : MonoBehaviour
{
    List<GameObject> zoneList;

    private void Start()
    {
        ZoneUtility.onAwake += AddZone;
    }

    private void AddZone(GameObject zone)
    {
        zoneList.Add(zone);
    }
}
