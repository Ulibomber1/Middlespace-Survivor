using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneUtility : MonoBehaviour
{
    public delegate void OnAwake(GameObject zone);
    public static event OnAwake onAwake;
    private void Awake()
    {
        onAwake?.Invoke(this.gameObject);
    }
}
