using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddEnemyPoolUtility : MonoBehaviour
{
    //References

    //Internal Variables

    [SerializeField] private int poolNum;


    //User defined objects

    //Delegates

    public delegate void OnAwake(GameObject pool, int poolNumber);


    //Events

    public static event OnAwake onAwake;


    //Unity Methods
    private void Awake()
    {
        onAwake?.Invoke(this.gameObject, poolNum - 1);
    }


    //User-defined methods
}
