using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private string[] levels;

    private void Start()
    {
        levels = new string[]{
        "Tutorial",
        "Level1_Scene",
        "TreasureRoom",
        "Level2_Scene",
        "TreasureRoom",
        "Level3_Scene",
        "TreasureRoom",
        "Level4_Scene",
        "TreasureRoom",
        "Level5_Scene"};
    }

    public void LoadNextLevel()
    {
        if (true)
        {

        }
    }
}
