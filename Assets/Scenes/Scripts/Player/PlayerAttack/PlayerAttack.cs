using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerController))]
public class PlayerAttack : MonoBehaviour
{
    [Header("Configuração Padrão (Sem arma)")]
    [SerializeField] private float baseDamage = 5f;
    [SerializeField] private float baseBulletSpeed = 10f;
    [SerializeField] private float baseCoolDown = 1f;

    [Header("Referências")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerController controller;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float DistanceOffset = 0f;

    [Header("Feedback Visual (Game Feel)")]
    [SerializeField] private float attackShakeDuration = 0.1f;
    [SerializeField] private float attackShakeMagnitude = 0.2f;

    public Weapons CurrentWeapon { get; private set; }
    public bool CanAttack { get; private set; } = true;
    public Vector2 AimDirection { get; private set; }

    public float CurrentDamage => CurrentWeapon != null ? CurrentWeapon.WeaponDamage : baseDamage;
    public float CurrentBulletSpeed => CurrentWeapon != null ? CurrentWeapon.BulletSpeed : baseBulletSpeed;
    public float CurrentCoolDown => CurrentWeapon != null ? CurrentWeapon.WeaponCoolDown : baseCoolDown;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        controller = GetComponent<PlayerController>();

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    private void Update()
    {
        HandleAiming();
    }

    public void HandleAiming()
    {
        Vector2 mouseScreenPosition = Input.mousePosition;

        Vector2 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);

        AimDirection = (mouseWorldPosition - (Vector2)transform.position).normalized;
    }

    public void HandleAttackInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && CanAttack)
        {
            controller.ChangeState(EPlayerState.Attacking);
        }
    }

    public void ExecuteAttack()
    {
        if(!CanAttack) { return; }

        StartCoroutine(AttackCooldownRoutine());

        if(CameraFollow.Instance != null)
        {
            CameraFollow.Instance.Shake(attackShakeDuration, attackShakeMagnitude);
        }

        GameObject bulletObject = PoolSpawner.Instance.GetBullet(1);

        if (bulletObject != null)
        {
            
            Vector2 fireDirection = AimDirection;

            Vector3 spawnDirection = fireDirection;
            Vector3 SpawnOffset = spawnDirection * DistanceOffset;

            bulletObject.transform.position = transform.position + SpawnOffset;
            bulletObject.SetActive(true);

            if (bulletObject.TryGetComponent<PlayerBullet>(out PlayerBullet bulletScript))
            {
                bulletScript.Setup(direction: spawnDirection, speed: CurrentBulletSpeed, damage: CurrentDamage);
            }
        }
    }
    private System.Collections.IEnumerator AttackCooldownRoutine()
    {
        CanAttack = false; // Impede o ataque
        yield return new WaitForSeconds(CurrentCoolDown); // Espera o tempo do cooldown
        CanAttack = true; // Permite o ataque novamente
    }
    public void SetWeapon(Weapons newWeapon)
    {
        CurrentWeapon = newWeapon; 
    }
}