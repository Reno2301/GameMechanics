using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatValues : MonoBehaviour
{
    public FightingPlayerCombat playerCombat;
    public FightingPlayerMovement playerMove;
    public Text healthText;
    public Text strengthText;
    public Text speedText;

    // Update is called once per frame
    void Update()
    {
        healthText.text = playerCombat.maxHealth.ToString();
        strengthText.text = playerCombat.attackDamage.ToString();
        speedText.text = playerMove.moveSpeed.ToString();
    }
}
