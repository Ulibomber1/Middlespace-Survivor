using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolController : MonoBehaviour
{
    //References

    private GameObject playerReference;
    private GameObject gameManagerReference;
    [SerializeField] private Dictionary<int, GameObject> enemyPools;


    //Internal Variables

    private float currentTime = 0;
    private float spawnTime = 0;
    private int poolPermission;
    [SerializeField] private float maxTimeSeconds;
    [SerializeField] private int spawnRateSeconds = 5;
    [SerializeField] private int maxPoolPermission;
    [SerializeField] private List<int> activeEnemyCount;
    [SerializeField] private List<int> maxEnemyCounts;


    //User defined objects

    //Delegates

    public delegate void AddThisReference(GameObject enemyPoolController);


    //Events

    public static event AddThisReference OnAddEnemyPoolController;


    //Unity Methods

    protected virtual void Awake()
    {
        EnemyController.OnEnemyDiabled += DecrementActiveEnemyCount;
        AddEnemyPoolUtility.onAwake += AddEnemyPoolInstance;
        GameManager.OnBroadcastGameManager += SetGameManagerReference;

        OnAddEnemyPoolController?.Invoke(gameObject);
        SetPlayerReference();

        enemyPools = new Dictionary<int, GameObject>();
        maxPoolPermission = 0;
    }

    protected virtual void Update()
    {
        switch (GameManager.Instance.gameState)
        {
            case GameState.PLAYING:
                currentTime += Time.deltaTime;
                spawnTime += Time.deltaTime;
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

    protected virtual void OnApplicationQuit()
    {
        ClearEnemyPools();
    }

    protected virtual void OnDestroy()
    {
        AddEnemyPoolUtility.onAwake -= AddEnemyPoolInstance;
        ClearEnemyPools();
        poolPermission = 1;
    }


    //User-defined methods

    private void DecrementActiveEnemyCount(GameObject pool)
    {
        for (int i = 0; i < enemyPools.Count; i++)
        {
            if (enemyPools[i] == pool)
            {
                activeEnemyCount[i]--;
            }
        }
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

    public virtual void AddEnemyPoolInstance(GameObject pool, int poolNumber)
    {
        enemyPools.Add(poolNumber, pool);
        maxEnemyCounts.Add(pool.transform.childCount);
        activeEnemyCount.Add(0);
        maxPoolPermission++;
    }

    private void SetGameManagerReference (GameObject gameManager)
    {
        if (gameManagerReference == null)
            gameManagerReference = gameManager;
    }

    private void SetPlayerReference()
    {
        if (gameManagerReference == null)
        {
            Invoke("SetPlayerReference", 0.01f);
            return;
        }
        else
        {
            if (playerReference == null)
                playerReference = gameManagerReference.GetComponent<GameManager>().GetPlayerReference();
        }
    }
}
