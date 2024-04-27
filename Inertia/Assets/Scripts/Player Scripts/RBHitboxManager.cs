using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBHitboxManager : MonoBehaviour
{
    PlayerRigidbodyCombat playerCombat;

    private void Start()
    {
        playerCombat = GetComponentInParent<PlayerRigidbodyCombat>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 6) //Collides with enemy layer
        {
            other.gameObject.GetComponent<EnemyHealth>().InitialiseDamage(playerCombat.damage, playerCombat.attackTimer);
        }
    }
}
