using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUtility : MonoBehaviour
{
    public delegate void GameOverUIAwakeHandler(GameObject GameOverUI);
    public static event GameOverUIAwakeHandler OnGameOverUIAwake;

    private void Awake()
    {
        OnGameOverUIAwake?.Invoke(gameObject);
    }

}
