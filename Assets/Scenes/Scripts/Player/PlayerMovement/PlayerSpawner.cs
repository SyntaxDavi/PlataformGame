using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Vector3 StartPosition;
    private Transform playerTransform;

    private void Awake()
    {
        playerTransform = transform;
    }
    private void Start()
    {
        ResetPosition();
    }

    private void OnEnable()
    {
        GameEvents.OnRoomLoaded += OnRoomLoadedHandler;
        GameEvents.OnPlayerDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        GameEvents.OnRoomLoaded -= OnRoomLoadedHandler;
        GameEvents.OnPlayerDeath -= HandlePlayerDeath;
    }

    private void OnRoomLoadedHandler()
    {
        ResetPosition();
    }

    private void HandlePlayerDeath()
    {
        Debug.Log("PlayerSpawner ouviu o evento de morte. Resetando posição.");
        ResetPosition();
    }

    public void ResetPosition()
    {
        playerTransform.position = StartPosition;
        GameEvents.TriggerPlayerSpawned(transform);
    }
}
