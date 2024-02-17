using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxManager : MonoBehaviour
{
    PlayerCombat playerCombat;

    private void Start()
    {
        playerCombat = GetComponentInParent<PlayerCombat>();
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

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            playerCombat.targetEnemies.Remove(other.gameObject);
        }
    }
}
