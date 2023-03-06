using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { MAIN_MENU, PLAYING, LEVELED_UP, BUYING_EQUIPMENT, PAUSE_MENU }

public delegate void OnStateChangeHandler();

public class GameManager : MonoBehaviour
{
    protected GameManager() { }
    private static GameManager instance = null;
    public event OnStateChangeHandler OnStateChange;
    public GameState gameState { get; private set; }

    private bool isGameOver = false;

    private int enemyCount = 0;
    [SerializeField]
    private int[] maxEnemyCounts;
    private GameObject[] enemyPools = GameObject.FindGameObjectsWithTag("Enemy Pool");
    // private Dialogue;
    // private BulletController;
    // private EnemyController;
    // private PlayerController;
    // private MenuController;

    private void Start()
    {
        for (int i = 0; i < enemyPools.Length; i++)
            CountChildren(enemyPools[i], ref maxEnemyCounts[i]); ;
    }

    public static GameManager Instance
    {
        get
        {
            if (GameManager.instance == null)
            {
                DontDestroyOnLoad(GameManager.instance);
                GameManager.instance = new GameManager { };
            }
            return GameManager.instance;
        }
    }

    public void SetGameState(GameState state)
    {
        this.gameState = state;
        OnStateChange();
    }

    private void SaveGame() 
    { 
        // using PlayerPrefs, save player stats and settings.
    }

    private void LoadGame()
    {
        // using PlayerPrefs, load player stats and settings.
    }

    private void ChangeScene()
    {

    }

    private void  CountChildren(GameObject parent, ref int maxCount)
    {
        maxCount = parent.transform.childCount;
    }

    private void SpawnEnemies() 
    {
        
    }

    public void OnApplicationQuit()
    {
        GameManager.instance = null;
    }
}