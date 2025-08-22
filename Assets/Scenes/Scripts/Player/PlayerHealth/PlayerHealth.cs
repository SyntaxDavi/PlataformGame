using UnityEngine;
using System.Collections;
public class PlayerHealth : MonoBehaviour
{
    public GameObject HealthBackground;
    public GameObject PrefabHealth;

    public float MaxHealth = 100f;
    public float CurrentHealth;

    [Header("Configurações de invencibilidade")]
    [SerializeField] private float invincibilityDuration = 1.5f;
    [SerializeField] private float invincibilityBlinkSpeed = 0.1f;

    [Header("Configuração do Knockback")]
    [SerializeField] private float knockbackForce = 12f;
    [SerializeField] private float knockbackDuration = 0.4f;

    private bool isInvincible = false;
    private bool isQuiting = false;

    private PlayerController playerController;
    private SpriteRenderer spriteRenderer;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        GameEvents.OnPlayerDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerDeath -= HandlePlayerDeath;
    }

    private void OnApplicationQuit()
    {
        isQuiting = true;
    }

    public void Start()
    {
        CurrentHealth = MaxHealth;
        GameEvents.TriggerPlayerHealthChanged(CurrentHealth,MaxHealth);    
    }

    public void TakeDamage(float Damage, Transform damageSource)
    {
        if (isQuiting || isInvincible || CurrentHealth <= 0)
        {
            return;
        } 
       
        CurrentHealth -= Damage;
        CurrentHealth = Mathf.Max(0, CurrentHealth);
        GameEvents.TriggerPlayerHealthChanged(CurrentHealth, MaxHealth);

        if (CurrentHealth > 0)
        {
            Vector2 knockbackDirection = (transform.position - damageSource.position).normalized;
            knockbackDirection = (knockbackDirection + Vector2.up * 0.5f).normalized;

            Debug.DrawRay(damageSource.position, knockbackDirection * 5f, Color.red, 2f);

            playerMovement.ApplyKnockback(knockbackDirection, knockbackForce, knockbackDuration);
            playerController.ChangeState(EPlayerState.Hurt, knockbackDuration);

            StartCoroutine(InvincibilityRoutine());
        }
        else
        {
            GameEvents.TriggerPlayerDeath();
            playerController.ChangeState(EPlayerState.Dead); 
        }
    }

    private IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;

        float endTime = Time.time + invincibilityDuration;

        while (Time.time < endTime)
        {
            if (spriteRenderer != null) spriteRenderer.enabled = false;
            yield return new WaitForSeconds(invincibilityBlinkSpeed);

            if (spriteRenderer != null) spriteRenderer.enabled = true;
            yield return new WaitForSeconds(invincibilityBlinkSpeed);
        }

        if (spriteRenderer != null) spriteRenderer.enabled = true;

        isInvincible = false;
    }

    private void HandlePlayerDeath()
    {
        CurrentHealth = MaxHealth;
        GameEvents.TriggerPlayerHealthChanged(CurrentHealth, MaxHealth);
    }
}
