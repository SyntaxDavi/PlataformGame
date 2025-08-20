using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private CharacterStats characterStats;
    private float lastAttackTime = 0f;

    private void Awake()
    {
       characterStats = GetComponent<CharacterStats>();

        if (characterStats == null)
        {
            Debug.Log("CharacterStats null");
            return;
        }
   
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (Time.time >= lastAttackTime + (1f / characterStats.StatSheet.AttackSpeed))
            {
                PlayerHealth playerHealth = collision.collider.GetComponent<PlayerHealth>();

                if (playerHealth != null && characterStats != null && characterStats.StatSheet != null)
                {
                    playerHealth.TakeDamage(characterStats.StatSheet.AttackDamage);
                    lastAttackTime = Time.time; 
                }
            }
        }
    }
}
