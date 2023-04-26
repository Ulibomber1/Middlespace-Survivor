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
    [SerializeReference] private GameObject gameOverGUI;
    [SerializeReference] private GameObject MainMenuGUI;
    [SerializeReference] private GameObject HUDGUI;
    protected GameManager() { }
    private static GameManager instance = null;
    public event OnStateChangeHandler OnStateChange;
    public GameState gameState { get; private set; }

    private float currentTime = 0;
    private float spawnTime = 0;
    [SerializeField] private int maxPoolPermission;
    private int poolPermission;
    [SerializeField] private float maxTimeSeconds;
    [SerializeField] private int spawnRateSeconds = 5;
    [SerializeField] private int credits;

    private int totalActiveEnemyCount;
    [SerializeField] private List<int> activeEnemyCount;
    [SerializeField] private List<int> maxEnemyCounts;
    //[SerializeField] private List<GameObject> enemyPools;
    [SerializeField] private Dictionary<int, GameObject> enemyPools;

    private GameObject playerReference;
    private Camera MainCamera;
    // private Dialogue;

    private void CreditsHandler(int amount)
    {
        credits += amount;
    }

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
        SceneManager.sceneLoaded += OnSceneLoad;
        EnemyController.OnEnemyDiabled += DecrementActiveEnemyCount;

        enemyPools = new Dictionary<int, GameObject>();
        maxPoolPermission = 0;
    }
    private void Start()
    {

        SetGameState(GameState.MAIN_MENU);
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
        instance.gameState = state;
    }
    private void DecrementActiveEnemyCount(GameObject pool)
    {
        for (int i = 0; i < enemyPools.Count; i++)
        {
            if(enemyPools[i] == pool)
            {
                activeEnemyCount[i]--;
            }
        }
    }
    private void GameOver()
    {
        SetGameState(GameState.GAME_OVER);
        playerReference.SetActive(false);
        gameOverGUI.SetActive(true);
        HUDGUI.SetActive(false);
        Debug.Log("gameOverGUI.activeSelf = " + gameOverGUI.activeSelf);
    }
    private void OnPlayClicked()
    {
        EnemyPoolController.onAwake += AddEnemyPoolInstance;
        GameOverUtility.OnGameOverUIAwake += SetupGameOverUI;
        HUDUtility.OnHUDAwake += SetupHUDUI;
        SetGameState(GameState.PLAYING);
        SceneManager.LoadScene("SampleScene");
        PlayerController.OnPlayerDead += GameOver;
        MainCamera = Camera.main;
        CreditsDropController.OnCreditsPickedUp += CreditsHandler;
        credits = 0;
    }
    public delegate void TimerUpdateHandler(float timeInSeconds);
    public event TimerUpdateHandler OnTimerUpdate;
    private void SetupHUDUI(GameObject UI)
    {
        HUDGUI = UI;
        OnTimerUpdate?.Invoke(maxTimeSeconds);
    }
    private void SetupGameOverUI(GameObject UI)
    {
        gameOverGUI = UI;
        Button gameOverButton = GameObject.Find("Button").GetComponent<Button>();
        gameOverButton.onClick.AddListener(OnReturnToMainMenu);
        gameOverGUI.SetActive(false);
    }
    private void OnQuitButtonClicked()
    {
        Application.Quit();
    }
    private void OnSceneLoad(Scene scene, LoadSceneMode sceneMode)
    {
        switch (Instance.gameState)
        {
            case GameState.PLAYING:
                playerReference = GameObject.FindGameObjectWithTag("Player");
                SceneManager.sceneLoaded -= OnSceneLoad;
                break;
            case GameState.PAUSE_MENU:
            case GameState.LEVELED_UP:
            case GameState.BUYING_EQUIPMENT:
                break;
            case GameState.MAIN_MENU:
                MainMenuGUI = GameObject.Find("Main Menu UI Canvas");
                Button[] buttonsArray = MainMenuGUI.GetComponentsInChildren<Button>(true);
                Debug.Log("buttonsArray.Length = " + buttonsArray.Length);
                for (int i = 0; i < buttonsArray.Length; i++)
                {
                    switch (buttonsArray[i].name)
                    {
                        case "Play Button":
                            buttonsArray[i].onClick.AddListener(OnPlayClicked);
                            break;
                        case "Exit":
                            buttonsArray[i].onClick.AddListener(OnQuitButtonClicked);
                            break;
                        case "Options":
                            //buttonsArray[i].onClick.AddListener();
                            break;
                        case "Unlocks Button":
                            //buttonsArray[i].onClick.AddListener();
                            break;
                    }
                }
                SceneManager.sceneLoaded -= OnSceneLoad;
                break;
            case GameState.GAME_OVER:
                break;
        }
    }
    public void OnReturnToMainMenu()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
        PlayerController.OnPlayerDead -= GameOver;
        EnemyPoolController.onAwake -= AddEnemyPoolInstance;
        GameOverUtility.OnGameOverUIAwake -= SetupGameOverUI;
        HUDUtility.OnHUDAwake -= SetupHUDUI;
        ClearEnemyPools();
        currentTime = 0;
        poolPermission = 1;
        SetGameState(GameState.MAIN_MENU);
        SceneManager.LoadScene("Title Screen");
    }
    public void AddEnemyPoolInstance(GameObject pool, int poolNumber)
    {
        enemyPools.Add(poolNumber, pool);
        maxEnemyCounts.Add(pool.transform.childCount);
        activeEnemyCount.Add(0);
        maxPoolPermission++;
    }

    public void AddCredit(int toAdd)
    {
        credits += toAdd;
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
                enemySpawn -= new Vector3(0, 2, 0);
                child.SetActive(true);
                child.transform.position = enemySpawn;

                return;
            }
        }

    }
    private void ClearEnemyPools()
    {
        maxPoolPermission = 0;
        enemyPools.Clear();
        maxEnemyCounts.Clear();
        activeEnemyCount.Clear();
    }
    public void OnApplicationQuit()
    {
        ClearEnemyPools();
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
                OnTimerUpdate?.Invoke(maxTimeSeconds-currentTime);
                break;
            case GameState.GAME_OVER:
                
                break;
        }

        if (spawnTime > spawnRateSeconds) 
        {
            spawnTime -= spawnRateSeconds;
            // Check time
            float timePercent = currentTime / maxTimeSeconds;
            poolPermission = Mathf.CeilToInt((enemyPools.Count * timePercent) /*+ 0.5f*/);
            poolPermission = (int)Mathf.Clamp((float)poolPermission, 0.0f, (float)maxPoolPermission);
            for (int i = 0; i < poolPermission; i++)
            {
                Debug.Log("For loop reached! iteration " + i + ". enemyCount is " + maxEnemyCounts[i] + " & maxEnemyCounts at i is " + maxEnemyCounts[i]);
                if (i == enemyPools.Count - 1 && activeEnemyCount[i] < 1)
                {
                    SpawnEnemiesFromPool(enemyPools[i]);
                    activeEnemyCount[i] += 1;
                }
                else if (activeEnemyCount[i] < maxEnemyCounts[i] && i != enemyPools.Count - 1)
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