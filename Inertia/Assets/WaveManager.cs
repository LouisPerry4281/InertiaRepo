using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] float[] waveCredits;

    float waveNumber;
    float credits;
    float enemiesAlive;

    bool isFinalWave;

    private void Start()
    {
        //Refill credits from array

        //Start interval spawning (maybe invoke repeating or something?)
    }

    void SpawnEnemy(GameObject enemyToSpawn)
    {
        //Choose one of the spawn points and instantiate the chosen enemy there

        //Set enemy's stance to pursuit

        //Add enemy to list of enemies
    }

    void NextWave()
    {
        //Increase Wave No.
        //Refill credits from array

        //If length of credit array matches current wave, isFinalWave

        //Start interval spawning (maybe invoke repeating or something?)
    }
}
