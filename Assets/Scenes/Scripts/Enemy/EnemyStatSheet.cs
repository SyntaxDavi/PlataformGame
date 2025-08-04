using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyStatSheet", menuName = "Enemy/Stat Sheet")]
public class EnemyStatSheet : ScriptableObject
{
    [Header("Estatísticas Base")]
    public float MaxHealth = 100f;
    public float BaseSpeed = 3f;

    [Header("Combate")]
    public float AttackDamage = 10;
    public float AttackSpeed = 1f;

    [Header("Recompensas")]
    public int GoldToDrop = 10;
}
