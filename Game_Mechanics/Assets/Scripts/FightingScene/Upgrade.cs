using UnityEngine;

public class Upgrade : MonoBehaviour
{
    public GameObject player;
    public PlayerScript playerScript;

    public bool upgradePressed;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerScript>();
    }

    public void AddSpeed()
    {
        if (!upgradePressed)
        {
            playerScript.moveSpeed += 1f;
            upgradePressed = true;
        }
    }
    public void AddStrength()
    {
        if (!upgradePressed)
        {
            playerScript.attackDamage += 2;
            upgradePressed = true;
        }
    }

    public void AddHealth()
    {
        if (!upgradePressed)
        {
            playerScript.maxHealth += 20;
            upgradePressed = true;
        }
    }
}
