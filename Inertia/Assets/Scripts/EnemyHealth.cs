using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float health;
    bool isVulnerable = true;

    ParticleSystem deathParticle;

    private void Start()
    {
        deathParticle = GetComponentInChildren<ParticleSystem>();
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
        HealthCheck();

        yield return new WaitForSeconds(damageTimer);

        isVulnerable = true;
    }

    private void HealthCheck()
    {
        if (health <= 0)
        {
            deathParticle.Play();

            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}
