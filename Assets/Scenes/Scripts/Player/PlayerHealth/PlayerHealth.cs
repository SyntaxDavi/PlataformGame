using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public GameObject HealthBackground;
    public GameObject PrefabHealth;

    public float MaxHealth;
    public float CurrentHealth;

    private bool isQuiting = false;

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

        CurrentHealth -= Damage;
        CurrentHealth = Mathf.Max(0, CurrentHealth);

        GameEvents.TriggerPlayerHealthChanged(CurrentHealth, MaxHealth);

        if (CurrentHealth <= 0)
        {
            Debug.Log("TriggerPlayeDeath Chamado");
            GameEvents.TriggerPlayerDeath();
        }
    }
}
