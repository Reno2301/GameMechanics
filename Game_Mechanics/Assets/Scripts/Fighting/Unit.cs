using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public string unitName;
    public int unitLevel;

    public int attack1Damage;
    public int attack2Damage;

    public int attack1Stamina;
    public int attack2Stamina;

    public int currentStamina;
    public int maxStamina;
    public int staminaAdd;

    public int maxHP;
    public int currentHP;
    public int healthAdd;

    public Vector2 position;

    public int potion;

    PlayerController player;

    public void Awake()
    {
        player = GetComponent<PlayerController>();

        if (gameObject.tag == "Player")
        {
            unitName = "You";
            unitLevel = PlayerPrefs.GetInt("CurrentLevel") + 1;

            attack1Damage = PlayerPrefs.GetInt("attack1Damage");
            attack2Damage = PlayerPrefs.GetInt("attack2Damage");

            maxStamina = PlayerPrefs.GetInt("maxStamina");
            maxHP = PlayerPrefs.GetInt("maxHealth");
            currentStamina = maxStamina; 
            currentHP = maxHP;

            potion = 2;

            healthAdd = 50;
            staminaAdd = 50;
            attack1Stamina = 10;
            attack2Stamina = 15;
        }
    }

    public bool TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
            return true;
        else return false;
    }

    public void AddStamina(int stamina)
    {
        currentStamina += stamina;

        if(currentStamina >= maxStamina)
        {
            currentStamina = maxStamina;
        }
    }

    public void AddHealth(int health)
    {
        currentHP += health;

        if(currentHP >= maxHP)
        {
            currentHP = maxHP;
        }
    }
}
