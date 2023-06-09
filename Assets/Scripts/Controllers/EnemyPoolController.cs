using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolController : MonoBehaviour
{
    //Public
    public delegate void OnAwake(GameObject pool, int poolNumber);
    public static event OnAwake onAwake;

    //Private
    [SerializeField] private int poolNum;

    private void Awake()
    {
        onAwake?.Invoke(this.gameObject, poolNum - 1);
    }
}
