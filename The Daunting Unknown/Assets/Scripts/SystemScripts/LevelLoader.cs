using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator animator;
    public Animator musicAnimator;

    public void ReLoadLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().HP = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().maxHP;
    }
    public void LoadNextLevel()
    {
        string activeSceneName = SceneManager.GetActiveScene().name;
        if (activeSceneName != "TreasureRoom" && activeSceneName != "Tutorial" && activeSceneName != "MenuScene")
        {
            LoadALevel(3);
        }
        else
        {
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        }
    }
    public void LoadALevel(int index)
    {
        StartCoroutine(LoadLevel(index));
    }
    IEnumerator LoadLevel(int levelindex)
    {
        animator.SetTrigger("Start");
        musicAnimator.SetTrigger("FadeOut");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(levelindex);

        yield return new WaitForSeconds(3f);
    }
}