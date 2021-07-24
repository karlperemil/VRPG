using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject gameOver;

    // Start is called before the first frame update


    void Start()
    {
        
    }

    public void OnClickStart() {
        Debug.Log(" OnClickStart");
        this.GetComponent<Canvas>().enabled = false;

        GameSingleton.Instance.StartGame();
    }

    public void OnGameOver(){
        this.GetComponent<Canvas>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Text score;
    internal void ShowYouDied()
    {
        gameOver.SetActive(true);
        score.text = "Score: " + GameSingleton.Instance.enemiesKilled;
    }
}
