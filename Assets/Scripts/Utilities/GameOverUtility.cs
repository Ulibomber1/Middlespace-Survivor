using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUtility : MonoBehaviour
{
//Private
    private void Awake()
    {
        OnGameOverUIAwake?.Invoke(gameObject);
    }

//Public
    public delegate void GameOverUIAwakeHandler(GameObject GameOverUI);
    public static event GameOverUIAwakeHandler OnGameOverUIAwake;
}
