using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextText : MonoBehaviour
{
    public Text text;
    public Text intText;
    public Text deleteText;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            text.gameObject.SetActive(true);
            intText.gameObject.SetActive(true);
            deleteText.gameObject.SetActive(false);
        }
    }
}
