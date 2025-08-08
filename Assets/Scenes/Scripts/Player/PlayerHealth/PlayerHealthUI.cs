using UnityEngine;
using System.Collections.Generic;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("Configuração Visual")]
    public GameObject PrefabHealth;

    private List<GameObject> healthIcons = new List<GameObject>();
    private bool isQuitting = false;

    private void OnApplicationQuit()
    {
        isQuitting = true;
    }
    private void OnEnable()
    {
        GameEvents.OnPlayerHealthChanged += UpdateHealthUI;
    }
    private void OnDisable()
    {
        if (!isQuitting)
        {
            GameEvents.OnPlayerHealthChanged -= UpdateHealthUI;
        }
    }

    //Este método será chamado AUTOMATICAMENTE pelo evento.
    private void UpdateHealthUI(float currentHealth, float maxHealth)
    {
        if (isQuitting) { return; }

        if (healthIcons.Count != (int)maxHealth)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            healthIcons.Clear();
        }

        for (int i = 0; i < (int)maxHealth; i++)
        {
            GameObject newIcon = Instantiate(PrefabHealth, transform);
            healthIcons.Add(newIcon);
        }

        //Ativa/desativa os ícones com base na vida atual
        for(int i = 0; i < healthIcons.Count; i++)
        {
            healthIcons[i].SetActive(i < currentHealth);
        }
    }
   
}
