using System.Collections.Generic;
using UnityEngine;

public class RunManager : MonoBehaviour
{
    public static RunManager Instance { get; private set; }

    [Header("Configuração da Run")]
    [Tooltip("Lista ORDENADA dos biomas do jogo")]
    [SerializeField] private List<BiomeData> biomeProgression;
    [SerializeField] private PlayerSpawner playerSpawner;
    [SerializeField] private SRPlataformSpawner plataformSpawner;

    private int currentBiomeIndex = 0;
    private int roomsClearedInBiome = 0;
    private bool hasLoadedMainRoom = false;
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
    private void OnEnable()
    {
        Debug.Log("RunManager se inscrevendo no evento OnPlayerReachedRoomExit.");
        GameEvents.OnPlayerReachedRoomExit += HandlePlayerReachedEndOfRoom;
    }

    private void OnDisable()
    {
        Debug.Log("RunManager se desinscrevendo do evento.");
        GameEvents.OnPlayerReachedRoomExit -= HandlePlayerReachedEndOfRoom;
    }

    private void HandlePlayerReachedEndOfRoom()
    {
        Debug.Log("RunManager ouviu o evento PlayerReachedEndOfRoom. Carregando próxima sala...");
        LoadNextRoom();
    }
    private void Start()
    {
        StartNewRun();
    }
    private void StartNewRun()
    {
        Debug.Log("Iniciando nova run!");
        GameEvents.TriggerRunStarted();
        currentBiomeIndex = 0;
        roomsClearedInBiome = 0;
        hasLoadedMainRoom = false;
        LoadNextRoom();
    }

    private void LoadNextRoom()
    {
        BiomeData currentBiome = biomeProgression[currentBiomeIndex];

        if (!hasLoadedMainRoom)
        {
            Debug.Log("Carregando MainRoom");
            plataformSpawner.SpawnLayout(currentBiome.mainRoom.layoutPrefab);
            hasLoadedMainRoom= true;
            return;
        }

        // Lógica de decisão de qual sala carregar
        // TODO: Adicionar lógica para Loja e MiniBoss

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
            availableCombatRooms.RemoveAt(randomIndex); //remove para nao repetir tãoo cedo

            plataformSpawner.SpawnLayout(chosenRoom.layoutPrefab);
            roomsClearedInBiome++;
            GameEvents.TriggerRoomLoaded();
        }
    }

    // Você vai precisar de um método público no seu SRPlataformSpawner
    // para que o RunManager possa mandar ele spawnar um layout específico.
}
