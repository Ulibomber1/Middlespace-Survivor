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
    private float startTime;
    private float currentTime = 0;

    private int enemyCount = 0;
    [SerializeField]
    private int[] maxEnemyCounts;
    private GameObject[] enemyPools;
    private GameObject playerReference;
    private Camera MainCamera;
    // private Dialogue;
    // private BulletController;
    // private EnemyController;
    // private PlayerController;
    // private MenuController;

    private void Start()
    {
        playerReference = GameObject.FindGameObjectWithTag("Player");
        enemyPools = GameObject.FindGameObjectsWithTag("Enemy Pool");
        SetGameState(GameState.MAIN_MENU);
        startTime = Time.time;
        MainCamera = Camera.main;

        for (int i = 0; i < enemyPools.Length - 1; i++)
            CountChildren(enemyPools[i], ref maxEnemyCounts[i]);
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

        Vector3 position = playerReference.transform.position;

        //CHANGE THIS LATER!!! UNVERIFIED RANGES!
        Vector3 enemySpawn = new Vector3(position.x + Random.Range(50, 55), 0, position.z + Random.Range(100, 105));
        
        for (int i = 0; i < maxCount - 1; i++)
        {
            GameObject child = parent.transform.GetChild(i).gameObject;

            if (!child.activeInHierarchy)
            {
                child.SetActive(true);
                child.transform.position = enemySpawn;
                return;
            }
        }
    }

    private void SpawnEnemies(GameObject enemyPool) 
    {
        int childCount = enemyPool.transform.childCount;
    }

    public void OnApplicationQuit()
    {
        GameManager.instance = null;
    }

    private void Update()
    {
        //Remove this later!
        gameState = GameState.PLAYING;

        switch (gameState)
        {
            case GameState.PLAYING:
                currentTime += Time.deltaTime;
                break;
        }

        //Debugging
        Debug.Log(currentTime);
        if ((int)currentTime % 5 == 0) { 

            for (int i = 0; i < maxEnemyCounts.Length - 1; i++)
            {

                if (enemyCount < maxEnemyCounts[i])
                {
                    //Debugging
                    Debug.Log("Enemy should have spawned");
                    SpawnEnemies(enemyPools[i]);
                    break;
                }
            }
        }
    }
}