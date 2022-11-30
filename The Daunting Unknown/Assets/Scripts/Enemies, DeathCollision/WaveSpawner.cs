using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState { Spawning, Waiting, Counting };
    [System.Serializable]
    public class Wave
    {
        public GameObject[] enemies;
        public int count;
        public float rate;
    }

    public GameObject exit;
    public bool Completed = false;
    public Wave[] waves;
    public Transform[] spawnpoints;
    private int nextWave = 0;
    public float TimeBetweenWaves = 2f;
    private float waveCountdown;
    private float searchCountdown = 1f;
    private Chest chest;
    public SpawnState state = SpawnState.Counting;

    private void Start()
    {
        exit = GameObject.FindGameObjectWithTag("exit");
        if (SceneManager.GetActiveScene().name == "TreasureRoom")
        {
            chest = FindObjectOfType<Chest>();
        }
        else
        {
            if (spawnpoints.Length == 0)
            {
                Debug.Log("Ne si referencenal spawn points");
            }
            waveCountdown = TimeBetweenWaves;
        }
        
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "TreasureRoom")
            CompleteLevel();
        else
        {
            if (state == SpawnState.Waiting)
            {
                if (!EnemyIsAlive())
                {
                    WaveCompleted();
                }
                else
                {
                    return;
                }
            }

            if (waveCountdown <= 0)
            {
                if (state != SpawnState.Spawning)
                {
                    StartCoroutine(SpawnWave(waves[nextWave]));
                }
            }
            else
            {
                waveCountdown -= Time.deltaTime;
            }
        }
    }

    void CompleteLevel()
    {
        if (chest.Opened)
        {
            Completed = true;
            transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = true;
            exit.GetComponent<DoorAnimation>().moving = true;
        }
    }

    void WaveCompleted()
    {
        state = SpawnState.Counting;
        waveCountdown = TimeBetweenWaves;

        if (nextWave + 1 > waves.Length - 1)
        {
            Completed = true;
            transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = true;
            exit.GetComponent<DoorAnimation>().moving = true;
        }
        else
        {
            nextWave++;
        }
    }
    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;

        if (searchCountdown <= 0)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                return false;
            }
        }
        return true;
    }
    IEnumerator SpawnWave(Wave wave)
    {
        if (!Completed)
        {
            state = SpawnState.Spawning;

            for (int i = 0; i < wave.count; i++)
            {
                SpawnEnemy(wave.enemies[Random.Range(0, wave.enemies.Length)].transform);
                yield return new WaitForSeconds(1f / wave.rate);
            }

            state = SpawnState.Waiting;
            yield break;
        }
    }

    void SpawnEnemy(Transform Enemy)
    {
        Transform sp = spawnpoints[Random.Range(0, spawnpoints.Length)];
        Instantiate(Enemy, sp.position, Quaternion.identity);
    }
}