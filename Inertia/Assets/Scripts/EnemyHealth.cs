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
    [SerializeField] GameObject hitParticles;

    [SerializeField] Material baseMat;
    [SerializeField] Material flashMat;

    Rigidbody rb;
    GameManager gameManager;
    Animator playerAnimator;
    NavMeshAgent agent;
    MeshRenderer meshRenderer;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        playerAnimator = GameObject.Find("Neutral Idle").GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
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
        playerAnimator.speed = 0;
        playerAnimator.GetComponentInParent<Rigidbody>().velocity = Vector3.zero;
        yield return new WaitForSeconds(0.1f);
        playerAnimator.speed = 1;

        CinemachineShake.Instance.ShakeCamera(2, .1f);

        Instantiate(hitEffect, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
        Instantiate(hitParticles, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.Euler(-90, 0, 0));

        //Color baseMatColor = baseMat.color;
        //baseMat.color = Color.white;

        //yield return new WaitForSeconds(0.1f);

        //baseMat.color = baseMatColor;

        agent.updatePosition = false;
        agent.velocity = Vector3.zero;
        agent.ResetPath();

        rb.isKinematic = false;
        agent.enabled = false;

        Knockback();

        health -= damageToTake;
        if (HealthCheck())
            yield return null;

        yield return new WaitForSeconds(0.5f);

        rb.isKinematic = true;
        agent.enabled = true;
        agent.updatePosition = true;

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
