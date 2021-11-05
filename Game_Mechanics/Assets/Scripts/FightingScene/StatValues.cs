using UnityEngine;
using UnityEngine.UI;

public class StatValues : MonoBehaviour
{
    public GameObject player;
    public Text healthText;
    public Text strengthText;
    public Text speedText;

    PlayerScript playerScript;

    private void Start()
    {
        playerScript = player.GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = playerScript.maxHealth.ToString();
        strengthText.text = playerScript.attackDamage.ToString();
        speedText.text = playerScript.moveSpeed.ToString();
    }
}
