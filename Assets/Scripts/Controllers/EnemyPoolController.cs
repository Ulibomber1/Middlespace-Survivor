using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyPoolController : MonoBehaviour
{
    private void Awake()
    {
        //Not maintanable
        GameManager.Instance.onPoolLoaded(this.gameObject);
    }
}
