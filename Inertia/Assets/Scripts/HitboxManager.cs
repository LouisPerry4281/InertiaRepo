using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxManager : MonoBehaviour
{
    PlayerCombat playerCombat;
    PlayerMovement playerMovement;

    private void Start()
    {
        playerCombat = GetComponentInParent<PlayerCombat>();
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            playerCombat.targetEnemies.Add(other.gameObject);
            
            if (playerCombat.isAttacking)
            {
                playerCombat.AttackScanEnemy();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            playerMovement.insideEnemy = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            playerMovement.insideEnemy = false;

            playerCombat.targetEnemies.Remove(other.gameObject);
        }
    }
}
