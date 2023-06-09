using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUtility : MonoBehaviour
{
    //Public
    public delegate void GameOverUIAwakeHandler(GameObject GameOverUI);
    public static event GameOverUIAwakeHandler OnGameOverUIAwake;

    //Private
    private void Awake()
    {
        OnGameOverUIAwake?.Invoke(gameObject);
    }

}
