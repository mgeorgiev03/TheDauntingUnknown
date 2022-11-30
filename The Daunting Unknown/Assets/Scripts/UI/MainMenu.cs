using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        //Player player = GetComponent<Player>();
        //player.LoadPlayer();
        FindObjectOfType<LevelLoader>().LoadNextLevel();
    }

    public void QiutGame()
    {
        Application.Quit();
    }
}
