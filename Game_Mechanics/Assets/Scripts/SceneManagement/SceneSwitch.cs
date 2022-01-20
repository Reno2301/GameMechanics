using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    public static string prevScene;
    public static string currentScene;

    TransitionAnimation tranAnim;

    public virtual void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
        tranAnim = GameObject.Find("SceneTransitionAnimation").GetComponent<TransitionAnimation>();
    }

    public void SwitchScene(string sceneName)
    {
        prevScene = currentScene;
        SceneManager.LoadScene(sceneName);
        tranAnim.SceneTransition();
    }
}

