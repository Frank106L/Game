using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerStatBar playerStatBar;
    [Header("事件监听")]
    public CharacterEventSO healthEvent;
    private void OnEnable()
    {
        healthEvent.OnEventRasied += OnHealthEvent;
    }
    private void OnDisable()
    {
        healthEvent.OnEventRasied -= OnHealthEvent;
    }

    private void OnHealthEvent(Character character)
    {
        var persentage = character.currentHealth / character.maxHealth;
        playerStatBar.OnHealthChange(persentage);
        playerStatBar.OnPowerChange(character);
    }
}
