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

    public List<GameObject> targetEnemies = new List<GameObject>();
    public List<GameObject> aliveEnemies = new List<GameObject>();

    [SerializeField] float attackRange;
    [SerializeField] float baseDamage;
    [SerializeField] float attackTimer;

    [SerializeField] float lockOnDashModifier;
    [SerializeField] float lockOffDashModifier;
    Vector3 dashVelocity;

    GameObject closestEnemy;
    float targetDistance;
    float closestEnemyDistance = 10000f;
    public bool inAttackRange = false;

    public bool isAttacking = false;

    private void Start()
    {
        _input = GetComponent<InputWrapper>();
        playerMovement = GetComponent<PlayerMovement>();
        _controller = GetComponent<CharacterController>();

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
        
        playerMovement.enabled = true;

        DistanceCheck();

        yield return null;
    }

    private void AttackDash()
    {
        //Disables collision between player and enemy layer
        Physics.IgnoreLayerCollision(7, 6, true);

        //This should be set to lock on facing movement
        Vector3 preDashDirection = closestEnemy.transform.position - gameObject.transform.position;
        dashVelocity = new Vector3(preDashDirection.x, 0, preDashDirection.z).normalized;
        dashVelocity = dashVelocity * lockOnDashModifier * Time.deltaTime;
    }

    private void AttackNormal()
    {
        print("NO ENEMY");
        /*
        //Disables collision between player and enemy layer
        Physics.IgnoreLayerCollision(7, 6, true);

        //This sould be turned to camera facing movement//////////////////////////////////
        Vector3 preDashVelocity = new Vector3(_controller.velocity.x, 0, _controller.velocity.z).normalized;
        dashVelocity = preDashVelocity * lockOnDashModifier * Time.deltaTime;*/
    }

    private void DashMovement()
    {
        print(dashVelocity);
        _controller.Move(dashVelocity);
    }

    private void DistanceCheck()
    {
        inAttackRange = false;

        foreach (GameObject enemy in aliveEnemies)
        {
            if (enemy == null)
                return;

            targetDistance = Vector3.Distance(enemy.transform.position, gameObject.transform.position);
            if (targetDistance < closestEnemyDistance)
            {
                closestEnemy = enemy;
                closestEnemyDistance = targetDistance;
            }
        }
        
        if (closestEnemy == null)
        {
            print("No Enemy In Range");
            inAttackRange = false;
            return;
        }
        if (Vector3.Distance(closestEnemy.transform.position, gameObject.transform.position) < attackRange)
        {
            inAttackRange = true;
        }

        else
            inAttackRange = false;
    }

    public void AddEnemiesToList(GameObject enemyToAdd)
    {
        aliveEnemies.Add(enemyToAdd);
    }

    public void RemoveEnemiesFromList(GameObject enemyToRemove)
    {
        aliveEnemies.Remove(enemyToRemove);
    }
}
