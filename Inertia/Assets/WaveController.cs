using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    [SerializeField] Transform[] spawnLocations;

    [SerializeField] GameObject enemy;

    [SerializeField] int[] enemiesToSpawn;

    GameManager gameManager;

    int enemiesLeftToSpawn;
    int currentWave = 0;

    float spawnIntervalTimer;
    [SerializeField] float spawnInterval;

    private void Start()
    {
        gameManager = GetComponent<GameManager>();

        enemiesLeftToSpawn = enemiesToSpawn[currentWave];
    }

    private void Update()
    {
        spawnIntervalTimer -= Time.deltaTime;

        if (enemiesLeftToSpawn > 0 && spawnIntervalTimer <= 0)
            SpawnEnemy();

        if (enemiesLeftToSpawn <= 0 && gameManager.enemyCount <= 0)
            NextWave();
    }

    public void SpawnEnemy()
    {
        int locationToSpawn = Random.Range(0, spawnLocations.Length);
        Instantiate(enemy, spawnLocations[locationToSpawn]);

        enemiesLeftToSpawn--;

        gameManager.enemyCount++;

        spawnIntervalTimer = spawnInterval;
    }

    public void NextWave()
    {
        currentWave++;

        if (currentWave >= 3)
        {
            print("End Game");
        }

        enemiesLeftToSpawn = enemiesToSpawn[currentWave];
    }
}
