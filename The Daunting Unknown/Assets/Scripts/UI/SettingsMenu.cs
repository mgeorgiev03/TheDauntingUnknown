using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public void SetVolume(float volume)
    {
    }

    public void SetQuality(int index)//from 0 to 5
    {
        QualitySettings.SetQualityLevel(index);
    }

    public void IsFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}
