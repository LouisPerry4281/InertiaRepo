using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    public void KillEnemy(GameObject enemyToKill)
    {
        Destroy(enemyToKill);

        //Adds 20% juice when an enemy is defeated
        player.GetComponent<PlayerRigidbodyMovement>().JuiceChange(0.2f);
    }
}
