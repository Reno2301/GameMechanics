using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePanel : MonoBehaviour
{
    public GameObject panel;
    public Rigidbody2D rb;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            bool isActive = panel.activeSelf;

            panel.SetActive(!isActive);

            rb.constraints = RigidbodyConstraints2D.FreezeAll;

            //stop animation
        }
    }
}
