using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OldBackToMenu : MonoBehaviour
{
    private void Update()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
