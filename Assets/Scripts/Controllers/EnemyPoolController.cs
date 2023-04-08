using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolController : MonoBehaviour
{
    public delegate void OnAwake(GameObject pool);
    public static event OnAwake onAwake;
    private void Awake()
    {
        onAwake?.Invoke(this.gameObject);
    }
}
