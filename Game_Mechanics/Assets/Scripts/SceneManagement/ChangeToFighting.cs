using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeToFighting : MonoBehaviour
{
    SceneSwitch sceneSwitch;

    [SerializeField] private string sceneName;

    DialogueManager dm;

    private void Start()
    {
        sceneSwitch = FindObjectOfType<SceneSwitch>();
        dm = FindObjectOfType<DialogueManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            sceneSwitch.SwitchScene(sceneName);
    }
}
