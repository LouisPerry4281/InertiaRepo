using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WaveManager : MonoBehaviour
{
    [SerializeField] float enemySpawnInterval;
    [SerializeField] float enemiesLeftForNextWave;

    [SerializeField] int[] waveCredits;

    [SerializeField] GameObject enemyPrefab;

    [SerializeField] Transform[] spawnPoints;

    SequenceManager sequenceManager;

    int waveNumber;
    int credits;
    public int enemiesAlive;

    public bool isFinalWave;

    private void Awake()
    {
        sequenceManager = GetComponent<SequenceManager>();
    }

    private void Start()
    {
        credits = waveCredits[waveNumber];

        StartCoroutine(SpawnEnemies());
    }

    private void Update()
    {
        enemiesAlive = sequenceManager.enemies.Count;

        if (enemiesAlive <= enemiesLeftForNextWave)
        {
            NextWave();
        }
    }

    IEnumerator SpawnEnemies()
    {
        for(int i = credits; i > 0; i--)
        {
            int spawnIndex = Random.Range(0, spawnPoints.Length);
            Vector3 spawnPoint = spawnPoints[spawnIndex].position;

            GameObject enemyInstance = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);

            enemyInstance.GetComponent<BobAI>().currentStance = BobAI.StanceSelector.Pursuit;

            sequenceManager.enemies.Add(enemyInstance.GetComponent<BobAI>());

            yield return new WaitForSeconds(enemySpawnInterval);
        }

        yield return null;
    }

    void NextWave()
    {
        waveNumber++;

        //If length of credit array matches current wave, it's the final wave
        if (waveCredits.Length == waveNumber + 1)
        {
            isFinalWave = true;
        }

        credits = waveCredits[waveNumber];

        StartCoroutine(SpawnEnemies());
    }
}
