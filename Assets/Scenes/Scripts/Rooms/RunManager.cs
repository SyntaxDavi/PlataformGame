using System.Collections.Generic;
using UnityEngine;

public class RunManager : MonoBehaviour
{
    public static RunManager Instance { get; private set; }

    [Header("Configura��o da Run")]
    [Tooltip("Lista ORDENADA dos biomas do jogo")]
    [SerializeField] private List<BiomeData> biomeProgression;

    [SerializeField] private PlayerSpawner spawner;
    [SerializeField] private SRPlataformSpawner plataformSpawner;

    private int currentBiomeIndex = 0;
    private int roomsClearedInBiome = 0;
    private List<RoomData> availableCombatRooms;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        StartNewRun();
    }
    private void StartNewRun()
    {
        Debug.Log("Iniciando nova run!");
        currentBiomeIndex = 0;
        roomsClearedInBiome = 0;
        LoadNextRoom();
    }

    public void PlayerReachedEndOfRoom()
    {
        Debug.Log("Jogador chegou ao final da sala. Carregando pr�xima...");
        roomsClearedInBiome++;
        LoadNextRoom();
    }

    private void LoadNextRoom()
    {
        BiomeData currentBiome = biomeProgression[currentBiomeIndex];

        // L�gica de decis�o de qual sala carregar
        // TODO: Adicionar l�gica para Loja e MiniBoss

        if(roomsClearedInBiome >= currentBiome.combatRoomsBeforeBoss)
        {
            //Sala do boss
            plataformSpawner.SpawnLayout(currentBiome.finalBossRoom.layoutPrefab);

            // vai para loja / lugar de descanso
        }
        else
        {
            if(availableCombatRooms == null || availableCombatRooms.Count == 0)
            {
                availableCombatRooms = new List<RoomData>(currentBiome.combatRooms);
            }

            int randomIndex = Random.Range(0, availableCombatRooms.Count);
            RoomData chosenRoom = availableCombatRooms[randomIndex];
            availableCombatRooms.RemoveAt(randomIndex); //remove para nao repitir t�oo cedo

            plataformSpawner.SpawnLayout(chosenRoom.layoutPrefab);
        }
        // TODO: Posicionar o jogador no ponto de in�cio da nova sala
        // playerSpawner.RespawnPlayerAtStartPoint();
    }

    // Voc� vai precisar de um m�todo p�blico no seu SRPlataformSpawner
    // para que o RunManager possa mandar ele spawnar um layout espec�fico.
}
