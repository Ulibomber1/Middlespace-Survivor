using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

class NullPlayer1 : PlayerController
{
    protected override void Awake() {; }
    protected override void Start() {; }
    protected override void Update() {; }
    protected override void FixedUpdate() {; }
    protected override void OnDestroy() {; }
    public override void InflictDamage(float damage) {; }
    public override void Heal(float healMod) {; }
    public override void OnMove(InputAction.CallbackContext context) {; }
    public override void OnLook(InputAction.CallbackContext context) {; }
    public override void OnFire(InputAction.CallbackContext context) {; }
    public override void OnDamage(InputAction.CallbackContext context) {; }
}
class NullPlayer2 : Player2Controller
{
    protected override void Awake() {; }
    protected override void Start() {; }
    protected override void Update() {; }
    protected override void FixedUpdate() {; }
    protected override void OnEnable() {; }
    protected override void OnCollisionEnter(Collision collision) {; }
    public override void OnMove(InputAction.CallbackContext context) {; }
    public override void OnLook(InputAction.CallbackContext context) {; }
    public override void OnFire(InputAction.CallbackContext context) {; }
    public override void OnDamage(InputAction.CallbackContext context) {; }
}
class NullEnemyPoolController : EnemyPoolController
{
    protected override void Awake() {; }
    protected override void Update() {; }
    protected override void OnApplicationQuit() {; }
    protected override void OnDestroy() {; }
    public override void AddEnemyPoolInstance(GameObject pool, int poolNumber) {; }
}

public enum GameState { MAIN_MENU, PLAYING, LEVELED_UP, BUYING_EQUIPMENT, PAUSE_MENU, GAME_OVER }

public delegate void OnStateChangeHandler();

static class ServiceLocator
{
    // Services
    private static PlayerController _playerController;
    private static Player2Controller _player2Controller;
    private static EnemyPoolController _enemyPoolController;
    // add ref for SoundManager, SaveSystem

    // null Services
    private static NullPlayer1 _nullPlayer1 = new();
    private static NullPlayer2 _nullPlayer2 = new();
    private static NullEnemyPoolController _nullEPController = new();

    public static PlayerController GetPlayer1() {return _playerController;}
    public static Player2Controller GetPlayer2() {return _player2Controller;}
    public static EnemyPoolController GetPool() {return _enemyPoolController;}

    // overloaded methods. Provides a service to this locator
    public static void Provide(PlayerController playerController) 
    {
        if (playerController == null)
        {
            _playerController = _nullPlayer1;
            Debug.LogWarning($"Service locator was given a null reference. Using null replacement.");
        }
        else
            _playerController = playerController; 
    }
    public static void Provide(Player2Controller player2Controller) 
    {
        if (player2Controller == null)
        {
            _player2Controller = _nullPlayer2;
            Debug.LogWarning($"Service locator was given a null reference. Using null replacement.");
        }
        else
            _player2Controller = player2Controller;
    }
    public static void Provide(EnemyPoolController enemyPoolController) 
    {
        if (enemyPoolController == null)
        {
            _enemyPoolController = _nullEPController;
            Debug.LogWarning($"Service locator was given a null reference. Using null replacement.");
        }
        else
            _enemyPoolController = enemyPoolController;
    }

    public static void Initialize()
    {
        _playerController = _nullPlayer1;
        _player2Controller = _nullPlayer2;
        _enemyPoolController = _nullEPController;
        Debug.Log($"Service Locator has been initialized.");
    }
}

public class GameManager : MonoBehaviour
{
    //References
    
    [SerializeReference] private GameObject gameOverGUI;
    [SerializeReference] private GameObject MainMenuGUI;
    [SerializeReference] private GameObject HUDGUI;
    [SerializeReference] private GameObject LevelUpUI;
    [SerializeReference] private GameObject EnemyPoolControllerReference;
    GameObject p2SpawnLocation;
    private GameObject playerReference;
    private GameObject player2Reference;
    private Camera MainCamera;
    protected GameManager() { }
    private static GameManager instance = null;

    //Internal Variables

    private float currentTime = 0;
    private float spawnTime = 0;
    [SerializeField] private float p2MaxSpawnTime;
    [SerializeField] private float maxTimeSeconds;
    private float p2SpawnTime;
    [SerializeField] private int credits = 0;
    private int totalActiveEnemyCount;


    //User defined objects

    private ItemDataUtility ItemData;


    //Delegates

    public delegate void CreditUpdateHandler(int amount);
    public delegate void ItemDataHandler(List<string> names);
    public delegate void TimerUpdateHandler(float timeInSeconds);
    public delegate void BroadcastGameManager(GameObject gameManager);


    //Events

    public event CreditUpdateHandler OnCreditsUpdated;
    public event ItemDataHandler OnDataReady; // might not need this
    public event TimerUpdateHandler OnTimerUpdate;
    public event OnStateChangeHandler OnStateChange;
    public static event BroadcastGameManager OnBroadcastGameManager;


    //Unity Methods

    private void Awake()
    {
        if (GameManager.instance == null)
        {
            GameManager.instance = this;
            DontDestroyOnLoad(GameManager.instance);
            ServiceLocator.Initialize();
        }
        else if (GameManager.instance != null && GameManager.instance != this)
        {
            Destroy(this);
        }
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void Start()
    {
        SetGameState(GameState.MAIN_MENU);
    }

    private void Update()
    {
        switch (gameState)
        {
            case GameState.PLAYING:

                if (playerReference == null)
                    playerReference = GameObject.FindGameObjectWithTag("Player");

                OnTimerUpdate?.Invoke(maxTimeSeconds - currentTime);
                break;
            case GameState.GAME_OVER:

                break;
        }
    }

    public void OnApplicationQuit()
    {
        GameManager.instance = null;
    }


    //User-defined methods

    private void PlayerOneJoined(GameObject player)
    {
        playerReference = player;
    }

    private void PlayerTwoJoined(GameObject player)
    {
        player2Reference = player;
        player2Reference.GetComponent<Player2Controller>().SetPlayerOneReference(playerReference);
        GameObject.Find("PlayerInputManager").SetActive(false);
        p2SpawnLocation = playerReference.transform.GetChild(3).gameObject;
        player2Reference.transform.position = p2SpawnLocation.transform.position;
        player2Reference.transform.rotation = p2SpawnLocation.transform.rotation.normalized;
    }

    private void Player2Despawn()
    {
        p2SpawnTime++;
        Invoke("Player2Spawn", p2SpawnTime);
    }

    private void Player2Spawn()
    {
        if (playerReference.activeSelf)
        {
            player2Reference.SetActive(true);
            player2Reference.transform.position = p2SpawnLocation.transform.position;
            player2Reference.transform.rotation = p2SpawnLocation.transform.rotation.normalized;
        }
    }

    private void GameOver()
    {
        //GameObject.Find("PlayerInputManager").SetActive(false);
        SetGameState(GameState.GAME_OVER);
        playerReference.SetActive(false);
        gameOverGUI.SetActive(true);
        HUDGUI.SetActive(false);
        Debug.Log("gameOverGUI.activeSelf = " + gameOverGUI.activeSelf);
    }

    private void OnPlayClicked()
    {
        GameOverUtility.OnGameOverUIAwake += SetupGameOverUI;
        HUDUtility.OnHUDAwake += SetupHUDUI;
        LevelUpUtility.OnAwake += SetUpLevelUpUI;
        LevelUpUtility.OnItemSelected += ResumeGame;
        PlayerController.OnLevelUp += LevelUp;
        CreditsDropController.OnCreditsPickedUp += AddCredit;
        PlayerController.OnPlayerJoined += PlayerOneJoined;
        Player2Controller.OnPlayerJoined += PlayerTwoJoined;
        SetGameState(GameState.PLAYING);
        SceneManager.LoadScene("SampleScene");
        PlayerController.OnPlayerDead += GameOver;
        MainCamera = Camera.main;
        Player2Controller.OnPlayer2Dead += Player2Despawn;
        p2SpawnTime = p2MaxSpawnTime;
        EnemyPoolController.OnAddEnemyPoolController += AddEnemyPoolController;
    }

    private void ResumeGame(string name) //string doesn't need to be used here
    {
        HUDGUI.SetActive(true);
        LevelUpUI.GetComponent<Canvas>().enabled = false;
        SetGameState(GameState.PLAYING);
    }

    private void SetUpLevelUpUI(GameObject ui)
    {
        LevelUpUI = ui;
        if (ui.TryGetComponent(out ItemDataUtility ItemDat))
            ItemData = ItemDat;
        else
            Debug.LogError("ItemDataUtility not found!");
        ui.GetComponent<Canvas>().enabled = false;
    }

    private void LevelUp(int newLevel)
    {
        if (newLevel <= 1)
            return;
        Debug.Log("LevelUp() reached.");
        SetGameState(GameState.LEVELED_UP);
        DisplayLevelUpScreen();
        PassItemData();
        if (ItemData == null)
            Debug.LogWarning("ItemData Empty!");
        Debug.Log("ItemData: " + ItemData);
    }

    private void PassItemData()
    {
        int i;
        List<string> names;
        names = ItemData.RandomItemIndices();
        Button[] buttons = LevelUpUI.GetComponentsInChildren<Button>();

        for (i = 0; i < 3; i++)
        {
            buttons[i].gameObject.GetComponent<MouseOver>().SetupButton(names[i]);
        }

        for (int j = 0; i < buttons.Length; i++, j++)
        {
            buttons[i].gameObject.GetComponent<MouseOver>().SetupButton(ItemData.GetItemNameByIndex(j));
        }

        OnDataReady?.Invoke(names);
    }

    private void DisplayLevelUpScreen()
    {
        HUDGUI.SetActive(false);
        LevelUpUI.GetComponent<Canvas>().enabled = true;
    }

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
                SceneManager.sceneLoaded -= OnSceneLoad;
                break;
            case GameState.PAUSE_MENU:
            case GameState.LEVELED_UP:
            case GameState.BUYING_EQUIPMENT:
                break;
            case GameState.MAIN_MENU:
                MainMenuGUI = GameObject.Find("Main Menu UI Canvas");
                Button[] buttonsArray = MainMenuGUI.GetComponentsInChildren<Button>(true);
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
                            //buttonsArray[i].onClick.AddListener(buttonsArray[i].GetComponent<MouseOver>().SetupButton());
                            break;
                    }
                }
                SceneManager.sceneLoaded -= OnSceneLoad;
                break;
            case GameState.GAME_OVER:
                break;
        }
    }

    private void SaveGame()
    {
        // using PlayerPrefs, save player stats and settings.
    }

    private void LoadGame()
    {
        // using PlayerPrefs, load player stats and settings.
    }

    [SerializeField] public GameState gameState { get; private set; }

    public static GameManager Instance
    {
        get
        {
            return GameManager.instance;
        }
    }

    public void AddCredit(int toAdd)
    {
        credits += toAdd;
        OnCreditsUpdated?.Invoke(credits);
    }

    public int GetCurrentCredits()
    {
        return credits;
    }
    
    public void SetGameState(GameState state)
    {
        instance.gameState = state;
    }

    public void OnReturnToMainMenu()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
        PlayerController.OnPlayerJoined -= PlayerOneJoined;
        Player2Controller.OnPlayerJoined -= PlayerTwoJoined;
        PlayerController.OnPlayerDead -= GameOver;
        PlayerController.OnLevelUp -= LevelUp;
        GameOverUtility.OnGameOverUIAwake -= SetupGameOverUI;
        HUDUtility.OnHUDAwake -= SetupHUDUI;
        LevelUpUtility.OnAwake -= SetUpLevelUpUI;
        LevelUpUtility.OnItemSelected -= ResumeGame;
        Player2Controller.OnPlayer2Dead -= Player2Despawn;
        CreditsDropController.OnCreditsPickedUp -= AddCredit;
        EnemyPoolController.OnAddEnemyPoolController -= AddEnemyPoolController;
        currentTime = 0;
        SetGameState(GameState.MAIN_MENU);
        SceneManager.LoadScene("Title Screen");
    }

    private void AddEnemyPoolController(GameObject enemyPoolController)
    {
        EnemyPoolControllerReference = enemyPoolController;
        BroadcastManager();
    }

    private void BroadcastManager()
    {
        OnBroadcastGameManager?.Invoke(gameObject);
    }

    public GameObject GetPlayerReference()
    {
        return playerReference;
    }
}