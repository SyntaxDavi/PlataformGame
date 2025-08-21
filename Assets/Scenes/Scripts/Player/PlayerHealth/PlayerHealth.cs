using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public GameObject HealthBackground;
    public GameObject PrefabHealth;

    public float MaxHealth = 100f;
    public float CurrentHealth;

    private bool isQuiting = false;

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

    public void TakeDamage(float Damage)
    {
        if (isQuiting) { return; }
        if (CurrentHealth <= 0) { return; }

        CurrentHealth -= Damage;
        CurrentHealth = Mathf.Max(0, CurrentHealth);

        GameEvents.TriggerPlayerHealthChanged(CurrentHealth, MaxHealth);

        if (CurrentHealth <= 0)
        {
            Debug.Log("TriggerPlayeDeath Chamado");
            GameEvents.TriggerPlayerDeath();
        }
    }

    private void HandlePlayerDeath()
    {
        CurrentHealth = MaxHealth;
        GameEvents.TriggerPlayerHealthChanged(CurrentHealth, MaxHealth);
    }
}
