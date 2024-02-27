using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private InputWrapper _input;
    PlayerMovement playerMovement;

    public List<GameObject> targetEnemies = new List<GameObject>();
    public List<GameObject> aliveEnemies = new List<GameObject>();

    [SerializeField] float attackRange;
    [SerializeField] float baseDamage;
    [SerializeField] float attackTimer;

    GameObject closestEnemy;
    float targetDistance;
    float closestEnemyDistance = 10000f;
    public bool inAttackRange = false;

    public bool isAttacking = false;

    private void Start()
    {
        _input = GetComponent<InputWrapper>();
        playerMovement = GetComponent<PlayerMovement>();

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
        _input.attack = false;
        isAttacking = true;

        playerMovement.enabled = false;

        DistanceCheck();

        /*if (inAttackRange)
            AttackDash();

        else
            AttackNormal();
        */
        yield return new WaitForSeconds(attackTimer);

        inAttackRange = false;

        isAttacking = false;

        playerMovement.enabled = true;

        yield return null;
    }

    private void AttackDash()
    {
        throw new NotImplementedException();
    }

    private void AttackNormal()
    {
        throw new NotImplementedException();
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
            print("No Closest Enemy");
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
