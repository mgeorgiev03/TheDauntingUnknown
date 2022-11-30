using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (SceneManager.GetActiveScene().name == "Level5_Scene")
            {
                FindObjectOfType<WinPanel>().Pause();
            }
            else
            {
                other.gameObject.GetComponent<PlayerMovement>().HP = 6;
                GameObject.Find("LevelLoader 2").GetComponent<LevelLoader>().LoadNextLevel();
            }
        }
    }
}
