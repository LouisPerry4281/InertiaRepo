using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptSequenceController : MonoBehaviour
{
    public int sequenceNumber = 1;
    /*
    1 - Before Combat
    2 - Combat Start
    3 - Combat End
    */

    public int enemyCount;

    [SerializeField] GameObject endZone;

    public void KillEnemy()
    {
        enemyCount--;

        if (enemyCount == 0)
        {
            sequenceNumber = 3;

            StartEndSequence();
        }
    }

    private void StartEndSequence()
    {
        endZone.SetActive(true);
    }

    public void SetSequence(int sequenceToSet)
    {
        sequenceNumber = sequenceToSet;
    }
}
