using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public float movementSpeed = 1f;

    public GameObject character;
    private EnemySpawner enemySpawner;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameSingleton.Instance.gameStarted == true){
            if(character != null){
                //transform.LookAt(character.transform.position);
                transform.LookAt( character.transform.position, Vector3.back ) ;
                //character.transform.rotation = Quaternion.Euler(character.transform.localRotation.eulerAngles.y, 0 , 0);
                transform.localRotation = Quaternion.Euler(0,transform.localRotation.eulerAngles.y,0);
                transform.position += transform.forward * Time.deltaTime * movementSpeed;
            }
        }
    }

    public void Init(EnemySpawner enemySpawner, GameObject character){
        this.character = character;
        this.enemySpawner = enemySpawner;
    }


    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player"){
            enemySpawner.RemoveEnemy(this);
            Destroy(this.gameObject);
            Debug.Log("Game over");

            GameSingleton.Instance.GameOver();
        }
        else if(other.gameObject.tag == "Bullet"){
            enemySpawner.RemoveEnemy(this);
            GameSingleton.Instance.EnemyKilled();
            Destroy(this.gameObject);
            Destroy(other.gameObject);
        }
    }
}
