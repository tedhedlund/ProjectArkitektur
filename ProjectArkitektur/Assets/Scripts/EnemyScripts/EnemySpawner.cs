using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Transform enemyPrefab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform areaUnlockObject;
    [SerializeField] Player_Controller player;
    [SerializeField] UIModelScript UIModelScript;
    public int numberOfKills = 0;

    public float timeBetweenWaves;
    public float countdown;
    
    private int waveNumber;
    private float currDist;

    private void Start()
    {
        waveNumber = Random.Range(1, 3);
    }

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
            if (waveNumber < 5)
            {
                waveNumber++;
            }

            if (currDist < 75f)
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
    }
}
