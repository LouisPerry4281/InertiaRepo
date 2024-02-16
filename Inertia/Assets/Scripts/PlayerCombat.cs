using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private InputWrapper _input;
    PlayerMovement playerMovement;

    public List<GameObject> targetEnemies = new List<GameObject>();

    [SerializeField] float baseDamage;
    [SerializeField] float attackTimer;

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
            g.GetComponent<EnemyHealth>().InitialiseDamage(finalDamage, attackTimer);
        }
    }

    IEnumerator Attack()
    {
        _input.attack = false;
        isAttacking = true;

        yield return new WaitForSeconds(attackTimer);

        isAttacking = false;

        yield return null;
    }
}
