using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LevelWindow : MonoBehaviour
{
    private Text levelText;
    private Text expText;
    private Slider expBarSlider;
    private LevelSystem levelSystem;

    private void Awake()
    {
        levelText = transform.Find("LevelText").GetComponent<Text>();
        expText = transform.Find("ExpText").GetComponent<Text>();
        expBarSlider = transform.Find("ExperienceBar").GetComponent<Slider>();
    }

    private void SetExperienceAmount(float experienceNormalized)
    {
        expBarSlider.value = experienceNormalized;
        expText.text = "EXPERIENCE :  " + (expBarSlider.value * levelSystem.experienceToNextLevel) + " / " + levelSystem.experienceToNextLevel;

        if(levelSystem.level >= 9)
        {
            expBarSlider.value = 1;
            expText.text = "EXPERIENCE : MAX";
        }
    }

    private void SetLevelNumber(int levelNumber)
    {
        levelText.text = "LEVEL :  " + (levelNumber + 1);
        if(levelSystem.level >= 9)
        {
            levelNumber = 9;
            levelText.text = "LEVEL: MAX";
        }
    }

    public void SetLevelSystem(LevelSystem levelSystem)
    {
        //Set the LevelSystem object
        this.levelSystem = levelSystem;

        //Update the starting values
        SetLevelNumber(levelSystem.GetLevelNumber());
        SetExperienceAmount(levelSystem.GetExperienceNormalized());

        //Subscribe to the changed events
        levelSystem.OnExperienceChanged += LevelSystem_OnExperienceChanged;
        levelSystem.OnLevelChanged += LevelSystem_OnLevelChanged;
    }

    private void LevelSystem_OnLevelChanged(object sender, System.EventArgs e)
    {
        //Level changed, update the text
        SetLevelNumber(levelSystem.GetLevelNumber());
    }

    private void LevelSystem_OnExperienceChanged(object sender, System.EventArgs e)
    {
        //Experience changed, update the bar
        SetExperienceAmount(levelSystem.GetExperienceNormalized());
    }
}
