using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnSystem : MonoBehaviour
{
    public GameObject[] spawnPoints = new GameObject[5];
    public GameObject[] spawnEnemyBox = new GameObject[2];
    public float spawnTimerInSeconds;

    public float enemyVelocityVector = 1.0f;

    private Vector3 spawnTimerPosition;
    private int whichSpawn;
    
    void Start()
    {
        StartCoroutine(SpawnTimer());
        StartCoroutine(SpawnDifficulty());        
    }    

    void InstantiateEnemy(int whichSpawnRef)
    {
        spawnTimerPosition = spawnPoints[whichSpawn].transform.position;
        int whichBox = Random.Range(0, 5);
        if (whichBox <= 3)
        {
            Instantiate(spawnEnemyBox[0], spawnTimerPosition, Quaternion.identity);
        }
        else
        {
            Instantiate(spawnEnemyBox[1], spawnTimerPosition, Quaternion.identity);
        }
    }

    IEnumerator SpawnTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnTimerInSeconds);
            whichSpawn = Random.Range(0, 5);            
            InstantiateEnemy(whichSpawn);                        
        }
    }

    IEnumerator SpawnDifficulty()
    {
        while (true)
        {
            yield return new WaitForSeconds(5.0f);
            enemyVelocityVector = enemyVelocityVector * 1.1f;
            spawnTimerInSeconds = spawnTimerInSeconds * 0.95f;
        }
    }
}
