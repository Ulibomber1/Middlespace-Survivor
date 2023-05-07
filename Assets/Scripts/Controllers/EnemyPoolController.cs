using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolController : MonoBehaviour
{
    [SerializeField] private int poolNum;

    public delegate void OnAwake(GameObject pool, int poolNumber);
    public static event OnAwake onAwake;
    private void Awake()
    {
        onAwake?.Invoke(this.gameObject, poolNum - 1);
    }
}
