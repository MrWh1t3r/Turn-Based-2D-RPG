using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image healthFill;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Character character;

    private void OnEnable()
    {
        character.OnHealthChange += OnUpdateHealth;
    }

    private void OnDisable()
    {
        character.OnHealthChange -= OnUpdateHealth;
    }

    private void Start()
    {
        OnUpdateHealth();
    }

    void OnUpdateHealth()
    {
        healthFill.fillAmount = character.GetHealthPercentage();
        healthText.text = character.curHp + "/" + character.maxHp;
    }
}
