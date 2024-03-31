using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    [SerializeField] Transform[] spawnLocations;

    [SerializeField] GameObject enemy;

    [SerializeField] int[] enemiesToSpawn;

    GameManager gameManager;
    SequenceController sc;

    int enemiesLeftToSpawn;
    public int currentWave = 0;
    
    float spawnIntervalTimer;
    [SerializeField] float spawnInterval;

    private void Start()
    {
        sc = FindAnyObjectByType<SequenceController>();

        enemiesLeftToSpawn = enemiesToSpawn[currentWave];
    }

    private void Update()
    {
        spawnIntervalTimer -= Time.deltaTime;

        if (enemiesLeftToSpawn > 0 && spawnIntervalTimer <= 0)
            SpawnEnemy();

        if (enemiesLeftToSpawn <= 0 && GameManager.enemyCount <= 2 && currentWave <= 1)
            NextWave();

        else if (currentWave == 2 && enemiesLeftToSpawn <= 0 && GameManager.enemyCount <= 0)
        {
            NextWave();
        }
    }

    public void SpawnEnemy()
    {
        int locationToSpawn = Random.Range(0, spawnLocations.Length);
        Instantiate(enemy, spawnLocations[locationToSpawn]);

        enemiesLeftToSpawn--;

        GameManager.enemyCount++;

        spawnIntervalTimer = spawnInterval;
    }

    public void NextWave()
    {
        currentWave++;

        if (currentWave <= 2 && GameManager.enemyCount <= 5)
            sc.IncrementSequence();

        if (currentWave >= 3)
        {
            sc.IncrementSequence();
            Destroy(this);
        }

        enemiesLeftToSpawn = enemiesToSpawn[currentWave];
    }
}
