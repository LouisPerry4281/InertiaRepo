using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float health;
    bool isVulnerable = true;

    [SerializeField] GameObject hitEffect;

    GameManager gameManager;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    public void InitialiseDamage(float damageToTake, float damageTimer)
    {
        if (isVulnerable)
            StartCoroutine(TakeDamage(damageToTake, damageTimer));
    }

    IEnumerator TakeDamage(float damageToTake, float damageTimer)
    {
        isVulnerable = false;

        health -= damageToTake;
        if (HealthCheck())
            yield return null;

        print("hit");
        Instantiate(hitEffect, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);

        yield return new WaitForSeconds(damageTimer);

        isVulnerable = true;
    }

    private bool HealthCheck()
    {
        if (health <= 0)
        {
            gameManager.KillEnemy(gameObject);
            return true;
        }

        return false;
    }
}
