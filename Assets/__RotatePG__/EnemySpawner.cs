using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Start is called before the first frame update



    public GameObject player;
    public GameObject enemyPrefab;
    private float spawnNextEnemyInSeconds = 3f;
    private float spawnRange = 10f;

    private List<GameObject> enemies = new List<GameObject>();

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float spawnTimeReduction = (Time.realtimeSinceStartup - GameSingleton.Instance.roundStartTime) * 0.001f;
        spawnTimeReduction = Mathf.Max(0.7f,spawnTimeReduction);
        if(GameSingleton.Instance.gameStarted == true){
            if(Time.realtimeSinceStartup > spawnNextEnemyInSeconds){
                SpawnEnemyRandomPosition();
                spawnNextEnemyInSeconds = Time.realtimeSinceStartup + Random.Range(1f - spawnTimeReduction,1.5f - spawnTimeReduction);
            }
        }
    }

    public void SpawnEnemyRandomPosition(){
            float randomSinVal = Random.Range(-7f,7f);
            Vector3 randomPosOnCircleEdge = new Vector3(Mathf.Sin(randomSinVal) * spawnRange,0f, Mathf.Cos(randomSinVal) * spawnRange);
            randomPosOnCircleEdge += player.transform.position;
            randomPosOnCircleEdge.y -= spawnRange*.5f;
            SpawnEnemy(randomPosOnCircleEdge);
    }

    public void SpawnEnemy(Vector3 spawnPosition){
        GameObject newEnemy = GameObject.Instantiate(enemyPrefab, this.transform);
        EnemyBehaviour enemyBehaviour = newEnemy.GetComponent<EnemyBehaviour>();
        enemyBehaviour.Init(this,player);
        newEnemy.transform.localPosition = spawnPosition;

        enemies.Add(newEnemy);
    }

    public void DestroyAllEnemies(){
        foreach (var item in enemies)
        {
            Destroy(item);
        }
    }

    internal void RemoveEnemy(EnemyBehaviour enemyBehaviour)
    {
        enemies.Remove(enemyBehaviour.gameObject);
    }
}
