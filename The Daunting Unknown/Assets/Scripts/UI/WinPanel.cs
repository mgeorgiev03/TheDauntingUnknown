using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinPanel : MonoBehaviour
{
    public GameObject winMenuUI;

    public void Pause()
    {
        winMenuUI.SetActive(true);
        Time.timeScale = 0f;
        FindObjectOfType<PauseMenu>().GameIsPaused = true;
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("Level1_Scene");
        Time.timeScale = 1f;
    }
    public void LoadMenu()
    {
        FindObjectOfType<LevelLoader>().LoadALevel(0);
        Time.timeScale = 1f;
    }
}
