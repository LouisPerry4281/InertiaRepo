using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerCombat : MonoBehaviour
{
    private InputWrapper _input;
    PlayerMovement playerMovement;
    CharacterController _controller;
    HitboxManager _hitboxManager;

    public List<GameObject> targetEnemies = new List<GameObject>();
    public List<GameObject> aliveEnemies = new List<GameObject>();

    [SerializeField] float attackRange;
    [SerializeField] float baseDamage;
    [SerializeField] float attackTimer;

    [SerializeField] float lockOnDashModifier;
    [SerializeField] float lockOffDashModifier;
    Vector3 dashVelocity;

    GameObject closestEnemyInRange;
    float closestEnemyDistance = 10000f;
    public bool inAttackRange = false;

    public bool isAttacking = false;

    [SerializeField] GameObject targetTestObject;

    private void Start()
    {
        _input = GetComponent<InputWrapper>();
        playerMovement = GetComponent<PlayerMovement>();
        _controller = GetComponent<CharacterController>();
        _hitboxManager = GetComponentInChildren<HitboxManager>();

        aliveEnemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
    }

    private void Update()
    {
        AttackCheck();
    }

    private void AttackCheck()
    {
        if (isAttacking)
        {
            DashMovement();
            AttackScanEnemy();
            return;
        }

        if (_input.attack) 
        {
            StartCoroutine(Attack());
        }
    }

    public void AttackScanEnemy()
    {
        foreach (GameObject g in targetEnemies)
        {
            float finalDamage = (baseDamage * playerMovement._currentJuice) + baseDamage / 2;
            if (g != null)
                g.GetComponent<EnemyHealth>().InitialiseDamage(finalDamage, attackTimer);
        }
    }

    IEnumerator Attack()
    {
        playerMovement.enabled = false;

        _input.attack = false;

        DistanceCheck();

        if (inAttackRange)
            AttackDash();

        else
            AttackNormal();

        isAttacking = true;

        yield return new WaitForSeconds(attackTimer);

        inAttackRange = false;

        isAttacking = false;
        playerMovement.isDashing = false;
        
        playerMovement.enabled = true;


        yield return null;
    }

    private void AttackDash()
    {
        //Disables collision between player and enemy layer
        Physics.IgnoreLayerCollision(7, 6, true);
        playerMovement.isDashing = true;

        //This should be set to lock on facing movement
        Vector3 preDashDirection = closestEnemyInRange.transform.position - gameObject.transform.position;
        dashVelocity = new Vector3(preDashDirection.x, 0, preDashDirection.z).normalized;

            

        dashVelocity = dashVelocity * lockOnDashModifier * Time.deltaTime;

        transform.rotation = Quaternion.LookRotation(preDashDirection);
    }

    private void AttackNormal()
    {
        Vector3 preDashDirection = Camera.main.transform.forward;
        preDashDirection.y = 0;
        transform.rotation = Quaternion.LookRotation(preDashDirection);

        dashVelocity = preDashDirection.normalized * lockOffDashModifier * Time.deltaTime;
                
        playerMovement.isDashing = true;
    }

    private void DashMovement()
    {
        _controller.Move(dashVelocity);
    }

    private void DistanceCheck()
    {
        //Variable Cleanups
        closestEnemyDistance = 10000f;
        closestEnemyInRange = null;

        foreach (GameObject enemy in aliveEnemies)
        {
            //Grabs the current distance between player and each iteration of enemy
            float currentDistance = Vector3.Distance(enemy.transform.position, gameObject.transform.position);

            //Checks to see if this is the closest enemy so far, then reassigns the new smallest distance to comparator
            if (currentDistance < closestEnemyDistance)
            {
                closestEnemyDistance = currentDistance;

                //Checks to see if closest enemy is in attack range and assigns it as such
                if (closestEnemyDistance < attackRange)
                {
                    closestEnemyInRange = enemy;
                    inAttackRange = true;
                }
            }
        }
    }

    public void AddEnemiesToList(GameObject enemyToAdd)
    {
        aliveEnemies.Add(enemyToAdd);
    }

    public void RemoveEnemiesFromList(GameObject enemyToRemove)
    {
        aliveEnemies.Remove(enemyToRemove);
        DistanceCheck();
    }
}
