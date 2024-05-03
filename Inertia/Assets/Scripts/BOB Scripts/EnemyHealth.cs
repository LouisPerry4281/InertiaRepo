using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] float knockbackForce;
    [SerializeField] float hitstopTime;
    [SerializeField] float stunTimer;
    bool isVulnerable = true;

    [SerializeField] GameObject hitEffect;
    [SerializeField] GameObject hitParticles;
    [SerializeField] GameObject sparkVFX;

    List<Material> baseMaterials = new List<Material>();
    MeshRenderer[] meshRenderers;
    [SerializeField] Material flashMat;

    

    Rigidbody rb;
    GameManager gameManager;
    Animator playerAnimator;
    NavMeshAgent agent;
    BobAI aiScript;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        playerAnimator = GameObject.Find("Neutral Idle").GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        aiScript = GetComponent<BobAI>();

        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach(MeshRenderer mr in meshRenderers)
        {
            baseMaterials.Add(mr.material);
        }
    }

    public void InitialiseDamage(float damageToTake, float damageTimer) //The public interface for starting the damage sequence
    {
        if (isVulnerable)
            StartCoroutine(TakeDamage(damageToTake, damageTimer));
    }

    IEnumerator TakeDamage(float damageToTake, float damageTimer)
    {
        isVulnerable = false; 

        aiScript.currentStance = BobAI.StanceSelector.Hurt; //Places the ai in a "stasis" like stance

        foreach(MeshRenderer mr in meshRenderers)
        {
            mr.material = flashMat;
        }

        AudioManager.instance.PlaySFX("MetalHit");

        //Hitstop stops the player's attack animation for a small time
        playerAnimator.speed = 0;
        playerAnimator.GetComponentInParent<Rigidbody>().velocity = Vector3.zero;
        yield return new WaitForSeconds(hitstopTime);
        playerAnimator.speed = 1;

        

        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material = baseMaterials[i];
        }

        CinemachineShake.Instance.ShakeCamera(3, .2f); //Camera Shake

        //Creates the particle effects and vfx
        Instantiate(hitEffect, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
        Instantiate(hitParticles, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.Euler(-90, 0, 0));
        Instantiate(sparkVFX, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);

        //Stops the navmesh agent controlling the enemy, and resets it's current movement and path
        agent.updatePosition = false;
        agent.velocity = Vector3.zero;
        agent.ResetPath();

        //Allows the rigidbody to take over, disabling the navmesh agent as they don't work well together
        rb.isKinematic = false;
        agent.enabled = false;

        Knockback(); //Knocks the enemy away from the player using rigidbody

        //Calculates damage and checks if player is dead
        health -= damageToTake;
        if (HealthCheck())
            yield return null;

        yield return new WaitForSeconds(stunTimer);

        //Re-enables the navmesh agent and disables rigidbody functionality
        rb.isKinematic = true;
        agent.enabled = true;
        agent.updatePosition = true;

        yield return new WaitForSeconds(damageTimer);

        aiScript.currentStance = BobAI.StanceSelector.Retreat; //Starts to retreat

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
        Vector3 direction = (playerAnimator.transform.position - transform.position).normalized;
        direction = new Vector3(direction.x, 0, direction.z);
        rb.AddForce(-direction * knockbackForce, ForceMode.Impulse);
    }
}
