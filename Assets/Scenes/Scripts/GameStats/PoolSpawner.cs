using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PoolConfig
{
    public PoolableType type;
    public GameObject prefab;
    public int size = 10;
    public bool canExpand = true;
}

public class PoolSpawner : MonoBehaviour
{
    public static PoolSpawner Instance;

    [Header("Pool Configurations")]
    [SerializeField] private List<PoolConfig> poolConfigurations;
    private Dictionary<PoolableType, Queue<GameObject>> pools;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        InitializePools();
       
    }
    private void InitializePools()
    {
        pools = new Dictionary<PoolableType, Queue<GameObject>>();

        foreach (var config in poolConfigurations) 
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();   
            
            for(int i = 0; i < config.size; i++)
            {
                GameObject obj = Instantiate(config.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            pools.Add(config.type, objectPool);
        }
    }

    public GameObject GetFromPool(PoolableType type)
    {
        if (!pools.ContainsKey(type))
        {
            Debug.LogWarning($"Pool: '{type}' não existe");
            return null;
        }

        GameObject objectToSpawn = pools[type].Dequeue();

        if(objectToSpawn == null)
        {
            PoolConfig config = poolConfigurations.Find(c => c.type == type);
            if(config != null && config.canExpand)
            {
                objectToSpawn = Instantiate(config.prefab);
            }
            else
            {
                pools[type].Enqueue(null);
                Debug.LogWarning($"Pool: '{type}' esta vazia e não pode expandir");
                return null;
            }
        }
        pools[type].Enqueue(objectToSpawn);

        objectToSpawn.SetActive(true);
        return objectToSpawn;
    }
}
