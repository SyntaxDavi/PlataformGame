using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Vector3 StartPosition;
    void Start()
    {
        ResetPosition();
    }

    public void ResetPosition()
    {
        transform.position = StartPosition;
    }
}
