using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] float knockbackForce;
    bool isVulnerable = true;

    [SerializeField] GameObject hitEffect;

    Rigidbody rb;
    GameManager gameManager;
    Animator playerAnimator;
    NavMeshAgent agent;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        playerAnimator = GameObject.Find("Neutral Idle").GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void InitialiseDamage(float damageToTake, float damageTimer)
    {
        if (isVulnerable)
            StartCoroutine(TakeDamage(damageToTake, damageTimer));
    }

    IEnumerator TakeDamage(float damageToTake, float damageTimer)
    {
        isVulnerable = false;

        //Hitstop Stuff
        /*playerAnimator.speed = 0;
        playerAnimator.GetComponentInParent<Rigidbody>().velocity = Vector3.zero;
        yield return new WaitForSeconds(0.2f);
        playerAnimator.speed = 1;*/

        Time.timeScale = 0.1f;
        playerAnimator.GetComponentInParent<Rigidbody>().velocity = Vector3.zero;
        yield return new WaitForSeconds(0.015f);
        Time.timeScale = 1;

        Instantiate(hitEffect, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);

        //agent.updatePosition = false;
        //agent.velocity = Vector3.zero;
        //agent.ResetPath();

        rb.isKinematic = false;
        agent.enabled = false;

        Knockback();

        health -= damageToTake;
        if (HealthCheck())
            yield return null;

        yield return new WaitForSeconds(0.5f);

        rb.isKinematic = true;
        agent.enabled = true;

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

    private void Knockback()
    {
        print("YEET");  
        Vector3 direction = (playerAnimator.transform.position - transform.position).normalized;
        direction = new Vector3(direction.x, 0, direction.z);
        rb.AddForce(-direction * knockbackForce, ForceMode.Impulse);
    }
}
