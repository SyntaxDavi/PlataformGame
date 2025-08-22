using System.Collections.Generic;
using UnityEngine;
using System.Collections;
enum BiomePhase
{
    MainRoom,
    PreShopCombats,
    Shop1,
    MiniBoss,
    PostMiniBossCombats,
    Shop2,
    FinalBoss,
    Transition
}
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
    private BiomePhase currentPhase = BiomePhase.MainRoom;

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
        GameEvents.OnPlayerDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        Debug.Log("RunManager se desinscrevendo do evento.");
        GameEvents.OnPlayerReachedRoomExit -= HandlePlayerReachedEndOfRoom;
        GameEvents.OnPlayerDeath -= HandlePlayerDeath;
    }

    private void HandlePlayerReachedEndOfRoom()
    {
        Debug.Log($"Fase atual: {currentPhase}. Carregando próxima sala...");
        LoadNextRoom();
    }

    private void HandlePlayerDeath()
    {
        Debug.Log("RunManager detectou a morte do jogador. Iniciando sequência de respawn...");
        StartCoroutine(RespawnSequenceCoroutine());
    }
    private IEnumerator RespawnSequenceCoroutine()
    {
        GameEvents.TriggerRespawnCycleStarted();
        yield return new WaitForSeconds(0.3f);

        Debug.Log("<Iniciando nova run>");
        currentBiomeIndex = 0;
        roomsClearedInBiome = 0;
        currentPhase = BiomePhase.MainRoom;

        if(availableCombatRooms != null)
        {
            availableCombatRooms.Clear();
        }

        GameEvents.TriggerRunStarted();
        LoadNextRoom();
    }
    private void Start()
    {
        StartCoroutine(StartNewRunSequence());
    }
    private IEnumerator StartNewRunSequence()
    {
        yield return null; 
        Debug.Log("Iniciando nova run!");
        currentBiomeIndex = 0;
        roomsClearedInBiome = 0;
        currentPhase = BiomePhase.MainRoom;

        if (availableCombatRooms != null)
        {
            availableCombatRooms.Clear();
        }

        GameEvents.TriggerRunStarted();
        LoadNextRoom();
    }

    private void LoadNextRoom()
    {
        if (biomeProgression.Count == 0 || currentBiomeIndex >= biomeProgression.Count)
        {
            Debug.LogError("Progresso de biomas esgotado ou não configurado.");

            GameEvents.TriggerRunCompleted();
            return;
        }

        BiomeData currentBiome = biomeProgression[currentBiomeIndex];

        switch (currentPhase)
        {
            case BiomePhase.MainRoom:

                roomsClearedInBiome = 0;
     
                availableCombatRooms = new List<RoomData>(currentBiome.combatRooms);
                LoadSpecialRoom(currentBiome.mainRoom, BiomePhase.PreShopCombats);
                break;

            case BiomePhase.PreShopCombats:

                if (roomsClearedInBiome < currentBiome.combatsBeforeShop1)
                {
                    LoadRandomCombatRoom(currentBiome);
                    roomsClearedInBiome++;
                }
                else
                {
                    currentPhase = BiomePhase.Shop1;
                    LoadNextRoom();
                }
                break;

            case BiomePhase.Shop1:

                LoadSpecialRoom(currentBiome.miniBossRoom, BiomePhase.MiniBoss);
                break;

            case BiomePhase.MiniBoss:

                roomsClearedInBiome = 0; 
                LoadSpecialRoom(currentBiome.miniBossRoom, BiomePhase.PostMiniBossCombats);
                break;

            case BiomePhase.PostMiniBossCombats:

                if (roomsClearedInBiome < currentBiome.combatsBeforeShop2)
                {
                    LoadRandomCombatRoom(currentBiome);
                    roomsClearedInBiome++;
                }
                else
                {
                    currentPhase = BiomePhase.Shop2;
                    LoadNextRoom();
                }
                break;

            case BiomePhase.Shop2:

                LoadSpecialRoom(currentBiome.finalBossRoom, BiomePhase.FinalBoss);
                break;

            case BiomePhase.FinalBoss:
      
                LoadSpecialRoom(currentBiome.finalBossRoom, BiomePhase.Transition);
                break;

            case BiomePhase.Transition:
                currentBiomeIndex++; // Avança para o próximo bioma

                if (currentBiomeIndex < biomeProgression.Count)
                {
                    Debug.Log($"Bioma {currentBiome.biomeName} concluído. Iniciando próximo bioma.");
                    // Prepara o próximo bioma, voltando para a MainRoom
                    currentPhase = BiomePhase.MainRoom;
                    LoadNextRoom();
                }
                else
                {
                    Debug.Log("Último bioma concluído. Run Vencida!");
                    GameEvents.TriggerRunCompleted();
                }
                break;
        }
    }
    private void LoadRandomCombatRoom(BiomeData currentBiome)
    {
        if (availableCombatRooms == null || availableCombatRooms.Count == 0)
        {
            // Se a lista de salas disponíveis acabar, re-popule com todas as salas do bioma.
            // Isso evita erros e permite que salas se repitam se o jogador passar por mais
            // salas do que as disponíveis.
            Debug.LogWarning("Todas as salas de combate foram usadas. Reciclando a lista.");
            availableCombatRooms = new List<RoomData>(currentBiome.combatRooms);
        }

        if (availableCombatRooms.Count == 0)
        {
            Debug.LogError($"O bioma '{currentBiome.biomeName}' não tem salas de combate configuradas!");
            return;
        }

        int randomIndex = Random.Range(0, availableCombatRooms.Count);
        RoomData chosenRoom = availableCombatRooms[randomIndex];
        availableCombatRooms.RemoveAt(randomIndex); 

        Debug.Log($"Carregando sala de combate: {chosenRoom.name}");
        plataformSpawner.SpawnLayout(chosenRoom.layoutPrefab);
        GameEvents.TriggerRoomLoaded();
    }
    private void LoadSpecialRoom(RoomData roomData, BiomePhase nextPhase)
    {
        if (roomData == null || roomData.layoutPrefab == null)
        {
            Debug.LogError($"Tentando carregar uma sala especial nula para a fase {nextPhase}. Verifique o BiomeData.");
            return;
        }

        Debug.Log($"Carregando sala especial: {roomData.name}. Próxima fase será: {nextPhase}");
        plataformSpawner.SpawnLayout(roomData.layoutPrefab);
        currentPhase = nextPhase;
        GameEvents.TriggerRoomLoaded();
    }
}