using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneUtility : MonoBehaviour
{
    public delegate void OnStart(GameObject zone);
    public static event OnStart onStart;
    private void Start()
    {
        onStart?.Invoke(gameObject);
    }
}
