using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [Header("Shot-Lifetime")]
    [SerializeField]private float timerAlive = 2f;

    private float bulletSpeed;
    private float bulletDamage;
    private Vector2 moveDirection;
    private float lifeTimer;

    public void OnEnable()
    {
        lifeTimer = 0f;
    }
    private void Update()
    {
        transform.Translate(moveDirection * bulletSpeed * Time.deltaTime);

        lifeTimer += Time.deltaTime;
        if(lifeTimer >= timerAlive)
        {
            gameObject.SetActive(false);
        }
    }
    public void Setup(float speed, float damage, Vector2 direction)
    {
        this.moveDirection = direction;
        this.bulletSpeed = speed;
        this.bulletDamage = damage;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Inimigo"))
        {
            CharacterStats Enemy = collision.GetComponent<CharacterStats>();

            if (Enemy != null)
            {
                Enemy.TakeDamage(bulletDamage);
            }
            gameObject.SetActive(false);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Debug.Log("Ground detectado");
            gameObject.SetActive(false);
        }
        else if (!collision.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }

    }
}