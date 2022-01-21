using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leveling : MonoBehaviour
{
    [SerializeField] private LevelWindow levelWindow;
    [SerializeField] private PlayerController player;

    private void Start()
    {
        LevelSystem levelSystem = new LevelSystem();
        levelWindow.SetLevelSystem(levelSystem);
        player.SetLevelSystem(levelSystem);
    }
}
