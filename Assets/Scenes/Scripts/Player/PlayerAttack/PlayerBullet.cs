using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [Header("Shot-Lifetime")]
    [SerializeField]private float LifeTime = 2f;
    public CharacterStats EnemyStats { get; set; }
    public PlayerAttack PlayerAttackInstance; 

    private float Timer;
    private Vector2 MoveDirection;

    public void OnEnable()
    {
        Timer = 0;
        MoveDirection = Vector2.zero;
        EnemyStats = null;
    }
    private void Update()
    {
        if (PlayerAttackInstance == null)
        {
            Debug.LogWarning("PlayerAttackInstance não foi setado!");
        }
        BulletLifetime();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Inimigo"))
        {
            CharacterStats Enemy = collision.GetComponent<CharacterStats>();
            if (Enemy != null)
            {
                Enemy.TakeDamage(PlayerAttackInstance.AttackDamage);
            }
            gameObject.SetActive(false);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            gameObject.SetActive(false);
        }
    }
    private void BulletLifetime()
    {
        if (PlayerAttackInstance != null) 
        {
            transform.Translate(PlayerAttackInstance.ShotBaseSpeed * Time.deltaTime * MoveDirection);
        }

        Timer += Time.deltaTime;

        if (Timer >= LifeTime)
        {
            gameObject.SetActive(false);
        }
    }
    public void SetDirection(Vector2 Direction)
    {
        MoveDirection = Direction.normalized;
    }
}