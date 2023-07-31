using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ServiceLocator : MonoBehaviour
{
    // Services
    private static PlayerController _playerController;
    private static Player2Controller _player2Controller;
    private static EnemyPoolController _enemyPoolController;
    // add ref for SoundManager, SaveSystem

    // null Services
    [SerializeField] private static NullPlayer1 _nullPlayer1;
    private static NullPlayer2 _nullPlayer2;
    private static NullEnemyPoolController _nullEPController;

    public static PlayerController GetPlayer1() { return _playerController; }
    public static Player2Controller GetPlayer2() { return _player2Controller; }
    public static EnemyPoolController GetPool() { return _enemyPoolController; }

    // overloaded method. Provides a service to this locator
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
        Debug.Log($"Service Locator has been initialized with null services.");
    }

    private void Awake()
    {
        _nullPlayer1 = gameObject.AddComponent<NullPlayer1>();
        _nullPlayer2 = gameObject.AddComponent<NullPlayer2>();
        _nullEPController = gameObject.AddComponent<NullEnemyPoolController>();
    }
}

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