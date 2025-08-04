using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private PlayerHealth Player;
    private CharacterStats characterStats;

    private void Awake()
    {
       characterStats = GetComponent<CharacterStats>();

        if (characterStats == null)
        {
            Debug.Log("CharacterStats null");
            return;
        }
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (Player != null && characterStats != null && characterStats.StatSheet != null)
            {
                Player.TakeDamage(characterStats.StatSheet.AttackDamage);
            }
            else
            {
                Debug.LogWarning("Tentativa de ataque falhou. Verifique as referências de Player ou CharacterStats/StatSheet.", gameObject);
            }
        }
    }
}
