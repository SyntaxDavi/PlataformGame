using UnityEngine;
using System.Collections.Generic;

public class PlayerHealth : MonoBehaviour
{
    public GameObject HealthBackground;
    public GameObject PrefabHealth;
    public float MaxHealth;
    public float CurrentHealth;

    private List<GameObject> healthIcons = new List<GameObject>();

    public void Start()
    {
        CurrentHealth = MaxHealth;

        foreach(Transform child in HealthBackground.transform)
        {
            Destroy(child.gameObject);
        }

        for(int i = 0; i < MaxHealth; i++)
        {
            GameObject newIcon = Instantiate(PrefabHealth, HealthBackground.transform);
            healthIcons.Add(newIcon);
        }

        LifeHudRefresh();
    }
    void LifeHudRefresh()
    {
       for(int i = 0; i < healthIcons.Count; i++)
        {
            if(i < CurrentHealth)
            {
                healthIcons[i].SetActive(true);
            }
            else
            {
                healthIcons[i].SetActive(false);
            }
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
