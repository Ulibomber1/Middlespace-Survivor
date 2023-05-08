using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZoneController : MonoBehaviour
{
    [SerializeField] public List<GameObject> zoneList;

    private void Awake()
    {
        ZoneUtility.onStart += AddZone;
    }

    private void AddZone(GameObject zone)
    {
        zoneList.Add(zone);
    }

    private void OnDestroy()
    {
        ZoneUtility.onStart -= AddZone;
    }
}
