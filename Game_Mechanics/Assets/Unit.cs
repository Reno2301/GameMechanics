using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public string unitName;
    public int unitLevel;

    public int attack1;
    public int attack2;

    public int currentStamina;
    public int maxStamina;

    public int maxHP;
    public int currentHP;

    public void Awake()
    {
        if (gameObject.tag == "Player")
        {
            maxStamina = PlayerPrefs.GetInt("maxStamina");
            currentStamina = maxStamina;
            maxHP = PlayerPrefs.GetInt("maxHealth");
            currentHP = maxHP;
        }
    }
}
