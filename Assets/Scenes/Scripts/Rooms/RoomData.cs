using UnityEngine;

public enum RoomType
{
    Combat,
    Shop,
    MiniBoss,
    Boss,
    StartRoom
}

[CreateAssetMenu(fileName = "New RoomData", menuName = "Roguelike/Room Data")]
public class RoomData : ScriptableObject
{
    [Tooltip("O prefab do layout desta sala.")]
    public GameObject layoutPrefab;

    [Tooltip("O tipo desta sala, usando pelo runManager para a lógica.")]
    public RoomType roomType;
}
