using System.Collections.Generic;
using UnityEngine;

public class SRPlataformSpawner : MonoBehaviour
{
    [Header("Configuração dos Layouts")]
    [Tooltip("Arraste todos os seus PREFABS de layout para esta lista.")]
    public List<GameObject> LayoutPrefabs;

    [Header("Dependências")]
    [Tooltip("Arraste o GameObject que contém o script EnemySpawner aqui.")]
    public EnemySpawner EnemySpawner;

    private GameObject CurrentLayoutInstance;
    private const string SPAWN_POINTS_CONTAINER_NAME = "EnemySpawnPoints";

    private void Start()
    {
        if (EnemySpawner == null)
        {
            return;
        }
        SpawnRandomLayout();
    }

    public void SpawnRandomLayout()
    {
        if (CurrentLayoutInstance != null)
        {
            Destroy(CurrentLayoutInstance);
        }

        if (LayoutPrefabs.Count == 0)
        {
            return;
        }

        int randomIndex = Random.Range(0, LayoutPrefabs.Count);
        GameObject chosenLayoutPrefab = LayoutPrefabs[randomIndex];
        CurrentLayoutInstance = Instantiate(chosenLayoutPrefab, transform.position, Quaternion.identity);
        Debug.Log(chosenLayoutPrefab);

        Transform SpawnPointsContainer = CurrentLayoutInstance.transform.Find(SPAWN_POINTS_CONTAINER_NAME);

        if(SpawnPointsContainer != null)
        {
            EnemySpawner.InitializeForLayout(SpawnPointsContainer);
            EnemySpawner.SpawnAllEnemies();
        }
    }
    public void SpawnSpecificLayout(GameObject layoutPrefab)
    {
        if (CurrentLayoutInstance != null)
        {
            Destroy(CurrentLayoutInstance);
        }

        if (layoutPrefab == null)
        {
            Debug.LogError("Recebeu um prefab de layout nulo para spawnar!");
            return;
        }

        CurrentLayoutInstance = Instantiate(layoutPrefab, transform.position, Quaternion.identity);

        Transform SpawnPointsContainer = CurrentLayoutInstance.transform.Find(SPAWN_POINTS_CONTAINER_NAME);

        if (SpawnPointsContainer != null)
        {
            EnemySpawner.InitializeForLayout(SpawnPointsContainer);
            EnemySpawner.SpawnAllEnemies();
        }
    }
}
