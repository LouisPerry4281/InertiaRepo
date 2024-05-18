using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage;
    public float attackTimer;

    BoxCollider triggerBox;

    private void Start()
    {
        triggerBox = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyHealth enemy = other.gameObject.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.InitialiseDamage(damage, attackTimer, true);
        }
    }

    public void EnableTrigger()
    {
        triggerBox.enabled = true;
    }

    public void DisableTrigger()
    {
        triggerBox.enabled = false;
    }
}
