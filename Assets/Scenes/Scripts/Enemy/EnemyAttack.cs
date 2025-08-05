using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private CharacterStats characterStats;

    private void Awake()
    {
       characterStats = GetComponent<CharacterStats>();

        if (characterStats == null)
        {
            Debug.Log("CharacterStats null");
            return;
        }
   
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.collider.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                if (characterStats != null && characterStats.StatSheet != null)
                {
                    playerHealth.TakeDamage(characterStats.StatSheet.AttackDamage);
                }
                else
                {
                    Debug.LogWarning("Ataque do inimigo falhou. Verifique CharacterStats/StatSheet.", gameObject);
                }
            }
        }
    }
}
