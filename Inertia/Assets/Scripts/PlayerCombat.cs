using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private InputWrapper _input;
    PlayerMovement playerMovement;

    public List<GameObject> targetEnemies = new List<GameObject>();
    public List<GameObject> enemiesInRange = new List<GameObject>();

    [SerializeField] float attackRange;
    [SerializeField] float baseDamage;
    [SerializeField] float attackTimer;

    GameObject closestEnemy;
    float closestEnemyDistance = 10000f;

    public bool isAttacking = false;

    private void Start()
    {
        _input = GetComponent<InputWrapper>();
        playerMovement = GetComponent<PlayerMovement>();
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

        //Checks all enemies to see which is closest
        enemiesInRange.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        foreach (GameObject enemy in enemiesInRange)
        {
            if (Vector3.Distance(gameObject.transform.position, enemy.transform.position) < closestEnemyDistance)
            {
                closestEnemy = enemy;
            }
        }
        print(closestEnemy.name);
        closestEnemyDistance = 10000f;

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

        yield return new WaitForSeconds(attackTimer);

        isAttacking = false;

        playerMovement.enabled = true;

        yield return null;
    }
}
