using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldClosePanel : MonoBehaviour
{
    public GameObject panel;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            bool isActive = panel.activeSelf;

            panel.SetActive(!isActive);
        }
    }
}
