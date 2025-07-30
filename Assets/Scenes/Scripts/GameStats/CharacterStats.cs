using UnityEngine;
using UnityEngine.Events;

public class CharacterStats : MonoBehaviour
{
    [Header("Configura��o")]
    [Tooltip("Arraste o Asset da ficha de Estat�stica do inimigo aqui.")]
    public EnemyStatSheet StatSheet;

    public float CurrentHealth {  get; private set; }
    public float CurrentSpeed { get; private set; }

    public EnemySpawner Spawner { get; set; }

    [Space]
    public UnityEvent<float> OnHeatlhChanged;
    public UnityEvent OnDie;

    private void OnEnable()
    {
        Initialize();
    }

    public void Initialize()
    {
        if(StatSheet == null)
        {
            Debug.LogError("Stat Sheet n�o est� atribu�da em " + gameObject.name);
            return;
        }

        CurrentHealth = StatSheet.MaxHealth;
        CurrentSpeed = StatSheet.BaseSpeed;

        OnHeatlhChanged?.Invoke(CurrentHealth);
    }

    public void TakeDamage(float Damage)
    {
        if (CurrentHealth <= 0) return;

        CurrentHealth -= Damage;
        CurrentHealth = Mathf.Max(CurrentHealth, 0); //Garente que a vida nao fique negativa

        //Anuncia que a vida mudou
        OnHeatlhChanged.Invoke(CurrentHealth);

        if(CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDie?.Invoke();

        if(Spawner != null)
        {
            Spawner.ReturnToPool(gameObject);
        }
        else
        {
            Destroy(gameObject, 3f);
        }
    }      
}
