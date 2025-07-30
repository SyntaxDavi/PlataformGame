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
            Debug.LogError("A referência ao EnemySpawner não está configurada no Inspector!");
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
            Debug.LogError("A lista de LayoutPrefabs está vazia! Adicione os prefabs no Inspector.");
            return;
        }

        // 1. Spawna o layout

        int randomIndex = Random.Range(0, LayoutPrefabs.Count);
        GameObject chosenLayoutPrefab = LayoutPrefabs[randomIndex];
        CurrentLayoutInstance = Instantiate(chosenLayoutPrefab, transform.position, Quaternion.identity);
        Debug.Log(chosenLayoutPrefab);

        // 2. Encontra o container de pontos de spawn DENTRO do layout que acabamos de criar
        Transform SpawnPointsContainer = CurrentLayoutInstance.transform.Find(SPAWN_POINTS_CONTAINER_NAME);

        // 3. Informa ao EnemySpawner sobre o novo container

        if(SpawnPointsContainer != null)
        {
            EnemySpawner.InitializeForLayout(SpawnPointsContainer);
            EnemySpawner.SpawnAllEnemies();
        }
        else
        {
            Debug.LogError($"Não foi possível encontrar o objeto filho '{SPAWN_POINTS_CONTAINER_NAME}' no prefab de layout '{chosenLayoutPrefab.name}'!");
        }
    }

}
