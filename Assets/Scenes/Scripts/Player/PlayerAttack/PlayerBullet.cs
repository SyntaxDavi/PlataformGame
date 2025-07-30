using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float Speed = 3;
    public float LifeTime = 0.5f;
    public float Damage {  get; set; }
    public CharacterStats EnemyStats { get; set; }

    private float Timer;
    private Vector2 MoveDirection;

    public void OnEnable()
    {
        Timer = 0;
    }
    private void Update()
    {
        BulletLifetime();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Inimigo"))
        {
            PlayerAttack.ResetCoolDown();

            CharacterStats Enemy = collision.GetComponent<CharacterStats>();
            if (Enemy != null)
            {
                Enemy.TakeDamage(Damage);
            }
            gameObject.SetActive(false);
        }

        if (collision.CompareTag("Chao"))
        {
            PlayerAttack.ResetCoolDown();
            gameObject.SetActive(false);
        }
    }
    private void BulletLifetime()
    {
        transform.Translate(MoveDirection * Speed * Time.deltaTime);
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
