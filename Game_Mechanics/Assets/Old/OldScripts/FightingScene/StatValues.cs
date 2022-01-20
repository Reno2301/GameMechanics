using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OldStatValues : MonoBehaviour
{
    public OldFightingPlayerCombat playerCombat;
    public OldFightingPlayerMovement playerMove;
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
