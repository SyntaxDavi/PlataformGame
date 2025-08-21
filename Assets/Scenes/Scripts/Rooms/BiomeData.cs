using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New BiomeData", menuName = "Roguelike/Biome Data")]
public class BiomeData : ScriptableObject
{
    [Header("Identificação do Bioma")]
    public string biomeName;

    [Header("Configurações de Salas de Combate")]
    [Tooltip("Lista de todas as salas de combate possíveis para este bioma.")]
    public List<RoomData> combatRooms;

    [Tooltip("Salas de combate antes da Loja 1.")]
    public int combatsBeforeShop1 = 3;
    [Tooltip("Salas de combate entre o MiniBoss e a Loja 2.")]
    public int combatsBeforeShop2 = 4;

    [Header("Salas Especiais")]
    public RoomData mainRoom;
    public RoomData shopRoom;
    public RoomData miniBossRoom;
    public RoomData finalBossRoom;
}
