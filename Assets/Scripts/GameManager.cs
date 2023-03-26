using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    public void OnPlayClicked()
    {
        SceneManager.LoadScene("SampleScene");
        SetGameState(GameState.PLAYING);

        startTime = Time.time;
        MainCamera = Camera.main;

    }

    public void onPoolLoaded(GameObject pool)
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

    private void SpawnEnemies(GameObject enemyPool) 
    {
        // only if in PLAYING state

        int childCount = enemyPool.transform.childCount;
        Vector3 position = playerReference.transform.position;

        Vector3 enemySpawn = new Vector3(position.x + Random.Range(10, 20), 0, position.z + Random.Range(10, 20));

        for (int i = 0; i < childCount - 1; i++)
        {
            GameObject child = enemyPool.transform.GetChild(i).gameObject;

            if (!child.activeInHierarchy)
            {
                child.SetActive(true);
                child.transform.position = enemySpawn;
                return;
            }
        }

    }

    public void OnApplicationQuit()
    {
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
                    SpawnEnemies(enemyPools[i]);
                    break;
                }
            }
        }
    }
}