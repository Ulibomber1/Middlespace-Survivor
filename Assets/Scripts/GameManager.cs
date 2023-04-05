using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public enum GameState { MAIN_MENU, PLAYING, LEVELED_UP, BUYING_EQUIPMENT, PAUSE_MENU, GAME_OVER }

public delegate void OnStateChangeHandler();

public class GameManager : MonoBehaviour
{
    private GameObject gameOverGUI;
    protected GameManager() { }
    private static GameManager instance = null;
    public event OnStateChangeHandler OnStateChange;
    public GameState gameState { get; private set; }

    private bool isGameOver = false;
    private float startTime;
    private float currentTime = 0;
    private float spawnTime = 0;
    [SerializeField] private int spawnRateSeconds = 5;

    private int totalActiveEnemyCount;
    [SerializeField] private List<int> activeEnemyCount;
    [SerializeField] private List<int> maxEnemyCounts;
    [SerializeField] private List<GameObject> enemyPools;

    private GameObject playerReference;
    private Camera MainCamera;
    // private Dialogue;
    // private BulletController;
    // private EnemyController;
    // private PlayerController;
    // private MenuController;

    private void Awake()
    {
        if (GameManager.instance == null)
        {
            GameManager.instance = this;
            DontDestroyOnLoad(GameManager.instance);
        }
        else if (GameManager.instance != null && GameManager.instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        SetGameState(GameState.MAIN_MENU);
        //SceneLoaded sceneLoadSubscribe = OnSceneLoaded;
    }

    public static GameManager Instance
    {
        get
        {
            return GameManager.instance;
        }
    }

    public void SetGameState(GameState state)
    {
        this.gameState = state;
    }
    private void SetPlayerRef(GameObject player)
    {
        playerReference = player;
    }
    private void SetGameOverRef(GameObject canvas)
    {
        gameOverGUI = canvas;
    }

    private void GameOver()
    {
        playerReference.SetActive(false);
        gameOverGUI.SetActive(true);
        SetGameState(GameState.GAME_OVER);
    }
    public void OnPlayClicked()
    {
        SceneManager.LoadScene("SampleScene");
        SceneManager.sceneLoaded += OnSceneLoad;
        SetGameState(GameState.PLAYING);
        EnemyPoolController.onAwake += AddEnemyPoolInstance;
        PlayerController.OnPlayerDead += GameOver;
        gameOverGUI = GameObject.Find("Game Over Screen");
        startTime = Time.time;
        MainCamera = Camera.main;
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode sceneMode)
    {
        playerReference = GameObject.Find("Player");
        gameOverGUI = GameObject.Find("Game Over Screen");
        gameOverGUI.SetActive(false);
    }

    public void OnReturnToMainMenu()
    {
        EnemyPoolController.onAwake -= AddEnemyPoolInstance;
        PlayerController.OnPlayerDead -= GameOver;
        SceneManager.LoadScene("Title Screen");
        SetGameState(GameState.MAIN_MENU);
    }

    public void AddEnemyPoolInstance(GameObject pool)
    {
        enemyPools.Add(pool);
        maxEnemyCounts.Add(pool.transform.childCount);
        activeEnemyCount.Add(0);
    }

    private void SaveGame() 
    { 
        // using PlayerPrefs, save player stats and settings.
    }

    private void LoadGame()
    {
        // using PlayerPrefs, load player stats and settings.
    }

    private void SpawnEnemiesFromPool(GameObject enemyPool) 
    {
        // only if in PLAYING state

        int childCount = enemyPool.transform.childCount;
        Vector3 position = playerReference.transform.position;
        int index;
        Vector3 enemySpawn;

        for (int i = 0; i < childCount; i++)
        {
            GameObject child = enemyPool.transform.GetChild(i).gameObject;

            if (!child.activeInHierarchy)
            {
                // chance for enemies to spawn at same zone, okay for now, FIX AFTER PROTOTYPING IS DONE
                index = Random.Range(0, playerReference.GetComponentInChildren<SpawnZoneController>().zoneList.Count);
                enemySpawn = playerReference.transform.Find("Spawn Zone Parent").GetChild(index).transform.position;
                child.SetActive(true);
                child.transform.position = enemySpawn;

                return;
            }
        }

    }

    public void OnApplicationQuit()
    {
        GameManager.instance.enemyPools.Clear();
        GameManager.instance.maxEnemyCounts.Clear();
        GameManager.instance.activeEnemyCount.Clear();
        GameManager.instance = null;
    }

    private void Update()
    {

        switch (gameState)
        {
            case GameState.PLAYING:

                if(playerReference == null)
                    playerReference = GameObject.FindGameObjectWithTag("Player");

                currentTime += Time.deltaTime;
                spawnTime += Time.deltaTime;
                break;
            case GameState.GAME_OVER:
                
                break;
        }

        if (spawnTime > spawnRateSeconds) 
        {
            spawnTime -= spawnRateSeconds;
            
            for (int i = 0; i < maxEnemyCounts.Count; i++)
            {
                Debug.Log("For loop reached! iteration " + i + ". enemyCount is " + maxEnemyCounts[i] + " & maxEnemyCounts at i is " + maxEnemyCounts[i]);
                if (activeEnemyCount[i] < maxEnemyCounts[i])
                {
                    //Debugging
                    Debug.Log("Enemy should have spawned");
                    SpawnEnemiesFromPool(enemyPools[i]);
                    activeEnemyCount[i] += 1;
                }
            }
        }
    }
}