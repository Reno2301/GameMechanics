using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quitgame : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Application.Quit();
            Debug.Log("Quit!");
        }
    }
}
