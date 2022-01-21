using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    public Text nameText;
    public Text levelText;
    public Slider healthSlider;
    public Slider staminaSlider;

    public void SetHUD(Unit unit)
    {
        nameText.text = unit.unitName;
        levelText.text = "LEVEL : " + unit.unitLevel;
        healthSlider.maxValue = unit.maxHP;
        healthSlider.value = unit.currentHP;

        staminaSlider.maxValue = unit.maxStamina;
        staminaSlider.value = unit.currentStamina;
    }

    public void SetHealthAndStamina(int hp, int stamina)
    {
        healthSlider.value = hp;
        staminaSlider.value = stamina;
    }
}
