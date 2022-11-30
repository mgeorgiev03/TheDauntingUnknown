using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private bool IsDead = false;
    public Animator animator;
    public GameObject gameOver;


    void Update()
    {
        if (!IsDead && !FindObjectOfType<PauseMenu>().GameIsPaused)
        {
            if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().HP <= 0)
            {
                IsDead = true;
                Invoke("Pause", 0.5f);
            }
        }  
    }
    void Pause()
    {
        gameOver.SetActive(true);
        Time.timeScale = 0f;
        FindObjectOfType<PauseMenu>().GameIsPaused = true;
    }
    public void Restart()
    {
        if (IsDead)
        {
            ReloadScene();
            Time.timeScale = 1f;
        }
    }
    public void LoadMenu()
    {
        FindObjectOfType<LevelLoader>().LoadALevel(0);
        Time.timeScale = 1f;
    }

    void ReloadScene()
    {
        FindObjectOfType<LevelLoader>().ReLoadLevel();
    }
}
