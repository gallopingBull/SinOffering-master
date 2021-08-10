using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public bool enableSpawn;
    public GameObject[] EnemyTypes;
    public GameObject[] SpawnLocs;
   

    public float SpawnRate = 10;
    public int MaxGroupSize = 3; //max count of enemies that can be spawned at a time 
    private int EnemyGroupSize; 

    private int enemyType;
    private int locId;

	// Use this for initialization
	void Start () {
        if(enableSpawn)
            EnableEnemySpawner();
	}

    public void EnableEnemySpawner()
    {
        InvokeRepeating("GenerateEnemy", 3, SpawnRate);
    }

    public void DisableSpawner()
    {
        CancelInvoke();
    }

    private int GetSpawnPoint()
    {
        return locId = Random.Range(0, SpawnLocs.Length);
    }
    
    private void IncreaseSpawnRate()
    {

    }

    public void GenerateEnemy()
    {
        EnemyGroupSize = Random.Range(0,MaxGroupSize);
        for (int i = 0; i < EnemyGroupSize; i++)
        {
            enemyType = Random.Range(0, EnemyTypes.Length);
            SpawnEnemy(enemyType);
        }
    }

    public void SpawnEnemy(int enemyIndex)
    {
        //offset the spawn location's x and y so enemies don't
        //spawn into eachother
        int tmpSpawnLoc = GetSpawnPoint();
        int offsetValueX = Random.Range(0, 3);
        int offsetValueY = Random.Range(0, 3);

        Vector3 finalLoc = 
            new Vector3(SpawnLocs[tmpSpawnLoc].transform.position.x + offsetValueX,
            SpawnLocs[tmpSpawnLoc].transform.position.y + offsetValueY,
            SpawnLocs[tmpSpawnLoc].transform.position.z);

        GameObject tmpEnemy = Instantiate(EnemyTypes[enemyIndex], 
            finalLoc, 
            SpawnLocs[tmpSpawnLoc].transform.rotation);
        /*
        //determine whether enemy should spawn aggro
        int randVal;
        randVal = Random.Range(0, 4);
        if (randVal % 2 == 0)
            tmpEnemy.GetComponent<EnemyController>().isAggro = true;
        */


        //varies the speed of the new enemy
        tmpEnemy.GetComponent<EnemyController>().Speed += offsetValueY;

        //
        if (tmpSpawnLoc != 0 && tmpSpawnLoc != 2)
            tmpEnemy.GetComponent<EnemyController>().facingLeft = true;
    }
}
