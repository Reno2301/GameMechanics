using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPanelInactive : MonoBehaviour
{
    public GameObject panel;

    public void Start()
    {
        panel.SetActive(false);
    }
}