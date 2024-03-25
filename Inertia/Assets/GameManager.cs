using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    GameObject player;

    ScriptSequenceController sequenceController;

    private void Start()
    {
        player = GameObject.Find("Player");
        sequenceController = FindAnyObjectByType<ScriptSequenceController>();
    }

    public void KillEnemy(GameObject enemyToKill)
    {
        enemyToKill.GetComponent<BoxCollider>().enabled = false;
        enemyToKill.GetComponent<BobAI>().enabled = false;
        enemyToKill.GetComponent<EnemyHealth>().enabled = false;
        enemyToKill.GetComponent<NavMeshAgent>().enabled = false;

        Destroy(enemyToKill);

        sequenceController.KillEnemy();

        //Adds 20% juice when an enemy is defeated
        player.GetComponent<PlayerRigidbodyMovement>().JuiceChange(0.4f);
    }

    public void KillPlayer()
    {
        print("ded");
    }
}
