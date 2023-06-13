using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZoneController : MonoBehaviour
{
    //References

    //Internal Variables

    [SerializeField] public List<GameObject> zoneList;


    //User defined objects

    //Delegates

    //Events

    //Unity Methods

    private void Awake()
    {
        ZoneUtility.onStart += AddZone;
    }


    //User-defined methods

    private void AddZone(GameObject zone)
    {
        zoneList.Add(zone);
    }

    private void OnDestroy()
    {
        ZoneUtility.onStart -= AddZone;
    }
}
