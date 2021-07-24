using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSingleton : MonoBehaviour
{
   private static GameSingleton _instance;

    public static GameSingleton Instance { get { return _instance; } }


    public bool gameStarted = false;
    public float roundStartTime = 0f;
    public Menu menu;

    public EnemySpawner enemySpawner;


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    internal void StartGame()
    {
        enemiesKilled = 0;
        gameStarted = true;
        roundStartTime = Time.realtimeSinceStartup;
    }

    internal void GameOver()
    {
        enemySpawner.DestroyAllEnemies();
        gameStarted = false;
        menu.GetComponent<Canvas>().enabled = true;
        menu.ShowYouDied();
    }

    public int enemiesKilled = 0;
    internal void EnemyKilled()
    {
        enemiesKilled++;
    }
}
