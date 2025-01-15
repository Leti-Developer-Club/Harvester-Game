using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;



//Scriptable Object to store level-specific prefabs
[CreateAssetMenu(fileName ="LevelPrefabs", menuName ="Game/Level Prefabs")]
public class LevelPrefabs : ScriptableObject
{
    public GameObject[] fruitPrefabs;
    public GameObject[] enemyPrefabs;
}

//Object pool manager
public class ObjectPooling : MonoBehaviour
{
    [System.Serializable]
    public class PoolSettings
    {
        //public string objectTag;
        public int initialSize;
        public int maxSize;
        public float spawnInterval;        
    }

    [Header("Pool Settings")]
    public PoolSettings fruitPoolSettings;
    public PoolSettings enemyPoolSettings;

    [Header("Level Configuration")]
    public LevelPrefabs levelPrefabs; 

    private Dictionary<string, Queue<GameObject>> fruitPool;
    private Dictionary<string, Queue<GameObject>> enemyPool;

    private float nextFruitSpawnTime;
    private float nextEnemySpawnTime;

    private float spawnWidth = 8f;
    private float spawnHeight = 6f;
    

    private void Start()
    {
        //create a disctionary
        fruitPool = new Dictionary<string, Queue<GameObject>>();
        enemyPool = new Dictionary<string, Queue<GameObject>>();

        InitializePools();

        //start spawning
        nextFruitSpawnTime = Time.time + fruitPoolSettings.spawnInterval;
        nextEnemySpawnTime = Time.time + enemyPoolSettings.spawnInterval;
    }

    private void InitializePools()
    {
        Debug.Log("Initializing object pools...");
        //initialize fruit pool
        foreach (GameObject prefab in levelPrefabs.fruitPrefabs)
        {
            Debug.Log($"Creating pool for fruit prefab: {prefab.name}");
            Queue<GameObject> pool = new Queue<GameObject>();
            for(int i = 0; i < fruitPoolSettings.initialSize; i++)
            {
                CreateNewPoolObject(prefab, pool);
            }
            fruitPool[prefab.name] = pool;
        }

        //initialize enemy pool
        foreach (GameObject prefab in levelPrefabs.enemyPrefabs)
        {
            Debug.Log($"Creating pool for enemy prefab: {prefab.name}");
            Queue<GameObject> pool = new Queue<GameObject>();
            for (int i = 0; i < enemyPoolSettings.initialSize; i++)
            {
                CreateNewPoolObject(prefab, pool);
            }
            enemyPool[prefab.name] = pool;
        }
        Debug.Log("Object pools initialized.");
    }

    private void CreateNewPoolObject(GameObject prefab, Queue<GameObject> pool)
    {
        GameObject obj = Instantiate(prefab);
        obj.SetActive(false);
        obj.transform.SetParent(transform);
        pool.Enqueue(obj);
    }

    public GameObject GetPooledObject(string prefabName, bool isFruit)
    {
        Debug.Log($"Attempting to retrieve a pooled object: {prefabName}, IsFruit: {isFruit}");

        Dictionary<string, Queue<GameObject>> targetPool;
        PoolSettings settings;

        if (isFruit)
        {
            targetPool = fruitPool;
            settings = fruitPoolSettings;
        }
        else
        {
            targetPool = enemyPool;
            settings = enemyPoolSettings;
        }


        if (!targetPool.ContainsKey(prefabName))
        {
            Debug.LogWarning($"No pool found for prefab: {prefabName}");
            Debug.Log($"No pool found for prefab: {prefabName}");
            return null;
        }

        Queue<GameObject> pool = targetPool[prefabName];

        //try to get an inactive object from the pool
        GameObject obj = null;
        while (pool.Count > 0 && obj == null)
        {
            obj = pool.Dequeue();
            //skip destroyed objects
            if (obj == null) continue; 
        }

        //create new object if needed or allowed
        if(obj == null && pool.Count < settings.maxSize)
        {
            GameObject prefab;
            Debug.Log($"Pool empty or object not available. Creating a new instance of {prefabName}.");

            if (isFruit)
            {
                prefab = System.Array.Find(levelPrefabs.fruitPrefabs, p => p.name == prefabName);
            }
            else
            {
                prefab = System.Array.Find(levelPrefabs.enemyPrefabs, p => p.name == prefabName);
            }

            if (prefab != null)
            {
                obj = Instantiate(prefab);
                obj.transform.SetParent(transform);
            }
        }

        if (obj != null)
        {
            Debug.Log($"Pooled object retrieved: {obj.name}");
            obj.SetActive(true);
        }

        return obj;

    }

    //return the object to the pool
    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);

        // Determine which pool to return to based on object name
        string objName = obj.name.Replace("(Clone)", "").Trim();

        if (fruitPool.ContainsKey(objName))
        {
            fruitPool[objName].Enqueue(obj);
            Debug.Log($"Returned object to fruit pool: {objName}");
        }
        else if (enemyPool.ContainsKey(objName))
        {
            enemyPool[objName].Enqueue(obj);
            Debug.Log($"Returned object to enemy pool: {objName}");
        }
        else
        {
            Debug.LogWarning($"Object {objName} does not belong to any pool.");
        }
    }

    private void Update()
    {
        // Handle fruit spawning
        if (Time.time >= nextFruitSpawnTime)
        {
            Debug.Log("Spawning random fruit...");
            SpawnRandomFruit();
            nextFruitSpawnTime = Time.time + fruitPoolSettings.spawnInterval;
        }

        // Handle enemy spawning
        if (Time.time >= nextEnemySpawnTime)
        {
            Debug.Log("Spawning random enemy...");
            SpawnRandomEnemy();
            nextEnemySpawnTime = Time.time + enemyPoolSettings.spawnInterval;
        }
    }

    private void SpawnRandomFruit()
    {
        if (levelPrefabs.fruitPrefabs.Length == 0)
        {
            Debug.LogWarning("No fruit prefabs available for spawning.");
            return;
        }

        GameObject prefab = levelPrefabs.fruitPrefabs[Random.Range(0, levelPrefabs.fruitPrefabs.Length)];
        GameObject fruit = GetPooledObject(prefab.name, true);

        if (fruit != null)
        {
            // Set random spawn position 
            fruit.transform.position = new Vector3(Random.Range(-spawnWidth, spawnWidth), Random.Range(spawnHeight, spawnHeight), 0f);
            Debug.Log($"Spawned fruit: {fruit.name} at {fruit.transform.position}");
        }
    }
    

    private void SpawnRandomEnemy()
    {
        if (levelPrefabs.enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("No enemy prefabs available for spawning.");
            return;
        }

        GameObject prefab = levelPrefabs.enemyPrefabs[Random.Range(0, levelPrefabs.enemyPrefabs.Length)];
        GameObject enemy = GetPooledObject(prefab.name, false);

        if (enemy != null)
        {
            // Set random spawn position            
            enemy.transform.position = new Vector3(Random.Range(-spawnWidth, spawnWidth), Random.Range(spawnHeight, spawnHeight), 0f);
            Debug.Log($"Spawned enemy: {enemy.name} at {enemy.transform.position}");
        }
    }

    // Call this when changing levels
    public void LoadLevelPrefabs(LevelPrefabs newLevelPrefabs)
    {
        Debug.Log("Loading new level prefabs...");

        // Clear existing pools
        foreach (var pool in fruitPool.Values)
        {
            while (pool.Count > 0)
            {
                GameObject obj = pool.Dequeue();
                if (obj != null) Destroy(obj);
            }
        }

        foreach (var pool in enemyPool.Values)
        {
            while (pool.Count > 0)
            {
                GameObject obj = pool.Dequeue();
                if (obj != null) Destroy(obj);
            }
        }

        fruitPool.Clear();
        enemyPool.Clear();

        // Set new prefabs and reinitialize
        levelPrefabs = newLevelPrefabs;
        InitializePools();

        Debug.Log("New level prefabs loaded.");
    }

}
