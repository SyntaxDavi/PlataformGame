using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public GameObject HealthBackground;
    public GameObject PrefabHealth;
    public float MaxHealth;
    public float CurrentHealth;

    public void Start()
    {
        CurrentHealth = MaxHealth;
        LifeHudRefresh();
    }
    void LifeHudRefresh()
    {
        foreach(Transform child in HealthBackground.transform)
        {
            Destroy(child);
        }

        for(int i = 0; i < CurrentHealth; i++)
        {
            Instantiate(PrefabHealth, HealthBackground.transform);
        }
    }

    public void TakeDamage(float Damage)
    {
        CurrentHealth -= Damage;
        CurrentHealth = Mathf.Max(0, CurrentHealth);
        LifeHudRefresh();

        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            gameObject.SetActive(false);
        }
    }
}
