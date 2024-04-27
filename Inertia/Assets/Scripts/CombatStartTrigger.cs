using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStartTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7) //Collides with player layer
        {
            GameManager.hasStartedCombat = true;
        }
    }
}
