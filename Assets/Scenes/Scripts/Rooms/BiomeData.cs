using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New BiomeData", menuName = "Roguelike/Biome Data")]
public class BiomeData : ScriptableObject
{
    [Header("Identificação do Bioma")]
    public string biomeName;

    [Header("Salas de Combate")]
    [Tooltip("Lista de todas as salas de combate possíveis para este bioma.")]
    public List<RoomData> combatRooms;

    [Header("Salas Especiais")]
    [Tooltip("Prefab da sala da loja aqui")]
    public RoomData ShopRoom;
    [Tooltip("O prefab da sala do MiniBoss.")]
    public RoomData miniBossRoom;

    [Header("Progresso")]
    [Tooltip("Quantas salas de combate o jogador deve vencer antes do chefe final.")]
    public int combatRoomsBeforeBoss = 10;
    public RoomData finalBossRoom;
}
