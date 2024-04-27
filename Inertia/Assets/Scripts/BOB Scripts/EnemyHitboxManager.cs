using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitboxManager : MonoBehaviour
{
    [SerializeField] float damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7) //When colliding with player
        {
            other.gameObject.GetComponent<PlayerRigidbodyHealth>().InitializeDamage(damage);
        }
    }
}
