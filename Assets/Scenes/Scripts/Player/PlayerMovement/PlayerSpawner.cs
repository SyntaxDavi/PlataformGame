using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Vector3 StartPosition;

    private void OnEnable()
    {
        GameEvents.OnRoomLoaded += OnRoomLoadedHandler;
    }

    private void OnDisable()
    {
        GameEvents.OnRoomLoaded -= OnRoomLoadedHandler;
    }

    private void OnRoomLoadedHandler()
    {
        ResetPosition();
    }

    public void ResetPosition()
    {
        transform.position = StartPosition;
    }
}
