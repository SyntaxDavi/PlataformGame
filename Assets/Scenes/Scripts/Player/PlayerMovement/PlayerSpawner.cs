using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Vector3 StartPosition;
    private Transform playerTransform;

    private void Awake()
    {
        playerTransform = transform;
    }

    private void OnEnable()
    {
        GameEvents.OnRoomLoaded += HandleRoomLoaded;
    }

    private void OnDisable()
    {
        GameEvents.OnRoomLoaded -= HandleRoomLoaded;
    }
    private void HandleRoomLoaded()
    {
        Debug.Log("Nova sala carregada. Reposicionando o jogador.");
        ResetPosition();
    }

    public void ResetPosition()
    {
        playerTransform.position = StartPosition;

        Debug.Log($"<color=green>Jogador reposicionado em {StartPosition}. Disparando evento OnPlayerSpawned!</color>");

        GameEvents.TriggerPlayerSpawned(playerTransform);
    }
}