using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyPoolConfig
{
    public GameObject Prefab;

    [Tooltip("A proporção deste inimigo na piscina. Um peso maior significa que ele será mais comum.")]
    [Range(1, 100)]

    public int Weight = 1;
}

public class EnemySpawner : MonoBehaviour
{
    [Header("Configuração dos Prefabs")]
    [Tooltip("Adicione todos os tipos de inimigos que este spawner pode criar.")]
    public List<EnemyPoolConfig> EnemyConfigs;

    [Header("Configuração do Pooling")]
    [Tooltip("Quantidade de inimigos a serem pré-carregados")]
    public int PoolSize = 20;

    private readonly Queue<GameObject> EnemyPool = new();
    private readonly List<Transform> SpawnPoints = new();
    private bool IsInitialized = false;

    private void Awake()
    {
        if(EnemyConfigs == null || EnemyConfigs.Count == 0)
        {
            Debug.Log("\"Nenhuma configuração de inimigo foi definida no EnemySpawner.");
        }

        CreateEnemyPool();
    }
    private void CreateEnemyPool()
    {
        List<GameObject> WeightedPrefabList = new List<GameObject>();

        foreach(var Config in EnemyConfigs)
        {
            for(int i = 0; i < Config.Weight; i++)
            {
                WeightedPrefabList.Add(Config.Prefab);
            }
        }

        if (WeightedPrefabList.Count == 0) return;

        for(int i = 0; i < PoolSize; i++)
        {
            int index = Random.Range(0, WeightedPrefabList.Count);
            GameObject EnemyInstance = Instantiate(WeightedPrefabList[index], transform);
            EnemyInstance.SetActive(false);
            EnemyPool.Enqueue(EnemyInstance);
        }
    }
    public void InitializeForLayout(Transform SpawnPointsContainer)
    {
        if (SpawnPointsContainer == null)
        {
            IsInitialized = false;
            return;
        }

        SpawnPoints.Clear();
        foreach (Transform Point in SpawnPointsContainer)
        {
            SpawnPoints.Add(Point);
        }

        ShuffleList(SpawnPoints);
        IsInitialized = true;
    }

    private void ShuffleList<T>(List<T> List)
    {
        for(int i = List.Count - 1; i > 0; i--)
        {
            int RandomIndex = Random.Range(0, i + 1);
            (List[i], List[RandomIndex]) = (List[RandomIndex], List[i]);
        }
    }

    public void SpawnAllEnemies()
    {
        if (!IsInitialized) return;

        int NumberToSpawn = Mathf.Min(SpawnPoints.Count, EnemyPool.Count);

        for (int i = 0; i < NumberToSpawn; i++)
        {
            // Pega o primeiro inimigo disponível da fila
            GameObject EnemyToSpawn = EnemyPool.Dequeue();

            if (EnemyToSpawn == null) return;

            EnemyToSpawn.transform.position = SpawnPoints[i].position;
            EnemyToSpawn.transform.rotation = SpawnPoints[i].rotation;
            EnemyToSpawn.SetActive(true);

        }
    }
    public void ReturnToPool(GameObject Enemy)
    {
        Enemy.SetActive(false);
        EnemyPool.Enqueue(Enemy);
    }
}
