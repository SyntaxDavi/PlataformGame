using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class EnemyDeathHandler : MonoBehaviour
{
    private CharacterStats characterStats;

    private void Awake()
    {
        characterStats = GetComponent<CharacterStats>();
    }

    private void OnEnable()
    {
        characterStats.OnDie.AddListener(HandleDeath);
    }

    private void OnDisable()
    {
        characterStats.OnDie.RemoveListener(HandleDeath);
    }

    private void HandleDeath()
    {

        InventoryManager.Instance.AddGold(characterStats.StatSheet.GoldToDrop);
        

        // Quer que ele exploda? Adicione a lógica aqui.
        // Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // Quer que ele toque um som? Adicione aqui.
        // AudioManager.PlaySound("EnemyDeathSound");
    }
}
