using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WaveManager : MonoBehaviour
{
    [SerializeField] float enemySpawnInterval;

    [SerializeField] int[] waveCredits;

    [SerializeField] GameObject enemyPrefab;

    [SerializeField] Transform spawnPoint; //Change this to an array later

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
    }

    IEnumerator SpawnEnemies()
    {
        for(int i = credits; i > 0; i--)
        {
            GameObject enemyInstance = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

            enemyInstance.GetComponent<BobAI>().currentStance = BobAI.StanceSelector.Pursuit;

            sequenceManager.enemies.Add(enemyInstance.GetComponent<BobAI>());

            yield return new WaitForSeconds(enemySpawnInterval);
        }

        yield return null;
    }

    void NextWave()
    {
        if (isFinalWave)
        {
            //If it was already the final wave, open the end game instead
            //Maybe bring this to the sequence manager and have it detect enemies remaining instead later
            sequenceManager.currentState = SequenceManager.SequenceState.EndOpen;
            return;
        }

        waveNumber++;
        credits = waveCredits[waveNumber];

        //If length of credit array matches current wave, it's the final wave
        if (waveCredits.Length == waveNumber)
        {
            isFinalWave = true;
        }

        StartCoroutine(SpawnEnemies());
    }
}
