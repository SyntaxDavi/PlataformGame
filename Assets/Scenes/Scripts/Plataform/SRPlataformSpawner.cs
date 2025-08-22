using System.Collections.Generic;
using UnityEngine;

public class SRPlataformSpawner : MonoBehaviour
{
    [Header("Dependências")]
    [Tooltip("Arraste o GameObject que contém o script EnemySpawner aqui.")]
    [SerializeField] private EnemySpawner enemySpawner;

    private GameObject CurrentLayoutInstance;
    private const string SPAWN_POINTS_CONTAINER_NAME = "EnemySpawnPoints";

    private void Start()
    {
        if (enemySpawner == null)
        {
            Debug.LogError("A referência ao EnemySpawner não foi configurada no SRPlataformSpawner!", this);
            enabled = false;
            return;
        }
    }

    public void SpawnLayout(GameObject layoutPrefab)
    {
        if(enemySpawner != null)
        {
            enemySpawner.DeactivateAndReturnAllEnemies();
        }

        if (CurrentLayoutInstance != null)
        {
            Destroy(CurrentLayoutInstance);
        }

        if (layoutPrefab == null)
        {
            Debug.LogError("O RunManager tentou spawnar um prefab de layout nulo!", this);
            return;
        }

        CurrentLayoutInstance = Instantiate(layoutPrefab, transform.position, Quaternion.identity);

        Transform spawnPointsContainer = CurrentLayoutInstance.transform.Find(SPAWN_POINTS_CONTAINER_NAME);

        if (spawnPointsContainer != null)
        {
            enemySpawner.InitializeForLayout(spawnPointsContainer);
            enemySpawner.SpawnAllEnemies();
        }
        else
        {
            Debug.LogWarning($"Layout '{layoutPrefab.name}' não contém um container de spawn de inimigos ('{SPAWN_POINTS_CONTAINER_NAME}'). Nenhum inimigo será spawnado.", this);
            enemySpawner.InitializeForLayout(null);
        }
    }
}
