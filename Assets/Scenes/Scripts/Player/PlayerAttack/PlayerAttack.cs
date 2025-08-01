using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Configuração Padrão (Sem arma)")]
    [SerializeField] private float baseDamage = 5f;
    [SerializeField] private float baseBulletSpeed = 10f;
    [SerializeField] private float baseCoolDown = 1f;

    [Header("Referências")]
    [SerializeField] private PlayerMovement playerMovement;

    [SerializeField] private float DistanceOffset = 0f;
    public Weapons CurrentWeapon { get; private set; }
    public bool IsPlayerAttacking { get; private set; }

    private float CoolDownTimer = 0f;

    public float CurrentDamage => CurrentWeapon != null ? CurrentWeapon.WeaponDamage : baseDamage;
    public float CurrentBulletSpeed => CurrentWeapon != null ? CurrentWeapon.BulletSpeed : baseBulletSpeed;
    public float CurrentCoolDown => CurrentWeapon != null ? CurrentWeapon.WeaponCoolDown : baseCoolDown;

    private void Awake()
    {
        if (playerMovement != null)
        {
            playerMovement = GetComponent<PlayerMovement>();
        }
    }

    private void Update()
    {
       if(CoolDownTimer > 0)
        {
            CoolDownTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && CanAttack())
        {
            playerMovement.SetAttackingState(true);
            Attack();
        }

        if (CoolDownTimer > 0)
        {
            CoolDownTimer -= Time.deltaTime;
            if (CoolDownTimer <= 0)
            {
                playerMovement.SetAttackingState(false);
            }
        }

    }
    private bool CanAttack()
    {
      return CoolDownTimer <= 0;
    }

    public void Attack()
    {
        CoolDownTimer = CurrentCoolDown;
        GameObject bulletObject = PoolSpawner.Instance.GetBullet(1);

        if (bulletObject != null)
        {
            Vector3 spawnDirection = (Vector3)playerMovement.FacingDirection;
            Vector3 SpawnOffset = spawnDirection * DistanceOffset;

            bulletObject.transform.position = transform.position + SpawnOffset;
            bulletObject.SetActive(true);

            PlayerBullet bulletScript = bulletObject.GetComponent<PlayerBullet>();
            if (bulletScript != null)
            {
                bulletScript.Setup(direction: spawnDirection, speed: CurrentBulletSpeed, damage: CurrentDamage);
            }
        }
    }
    public void SetWeapon(Weapons newWeapon)
    {
        CurrentWeapon = newWeapon; 
    }
}