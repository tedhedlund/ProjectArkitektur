using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Transform enemyPrefab;
    //[SerializeField] int maxSpawn;
    //List<Transform> enemyList= new List<Transform>();

    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform areaUnlockObject;
    [SerializeField] Player_Controller player;
    [SerializeField] UIModelScript UIModelScript;
    [SerializeField] int numberOfKills = 0;
    [SerializeField] float timeBetweenWaves;
    [SerializeField] float countdown;
    
    private int waveNumber;
    private float currDist;

    public void Update()
    {
        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
        }

        countdown -= Time.deltaTime;

        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
        currDist = Vector3.Distance(transform.position, player.transform.position);

        if (UIModelScript.curKills >= numberOfKills && areaUnlockObject != null)
        {
            Destroy(areaUnlockObject.gameObject);
        }
    }

    IEnumerator SpawnWave()
    {
        if (areaUnlockObject == null) 
        {
            //waveNumber++;

            if (waveNumber < 5)
            {
                waveNumber++;
            }

            if (currDist < 75f /*&& enemyList.Count <= maxSpawn*/)
            {
                for (int i = 0; i < waveNumber; i++)
                {
                    SpawnEnemy();
                    yield return new WaitForSeconds(0.75f);
                }
            }
        }
    }

    public void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        //var spawnedEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        //enemyList.Add(spawnedEnemy);
    }
}
