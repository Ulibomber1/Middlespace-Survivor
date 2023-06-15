using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Global Enums

public class ZoneUtility : MonoBehaviour
{
    // Private Enums

    // References

    // Internal Variables

    // User-Defined Objects

    // Delegates
    public delegate void OnStart(GameObject zone);

    // Events
    public static event OnStart onStart;

    // Unity Methods
    private void Start()
    {
        onStart?.Invoke(gameObject);
    }

    // User-Defined Methods
}
