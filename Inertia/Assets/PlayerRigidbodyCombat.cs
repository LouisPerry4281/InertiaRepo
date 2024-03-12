using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerRigidbodyCombat : MonoBehaviour
{
    PlayerRigidbodyMovement movement;

    [SerializeField] Collider hitBox;

    public float damage;
    public float attackTimer;

    public bool attackInput;
    bool isAttacking;

    private void Start()
    {
        movement = GetComponent<PlayerRigidbodyMovement>();
    }

    private void Update()
    {
        if (attackInput)
        {
            print("Att Input");
            if (CanAttack() && !isAttacking)
            {
                StartCoroutine(Attack());
            }
        }

        else
            attackInput = false; print("no Att");
    }

    IEnumerator Attack()
    {
        attackInput = false;
        isAttacking = true;

        hitBox.enabled = true;

        yield return new WaitForSeconds(attackTimer);

        hitBox.enabled = false;

        isAttacking = false;
    }

    private bool CanAttack()
    {
        //These are all the conditions for being able to attack
        if (!movement.isDashing)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    private void OnAttack(InputValue value)
    {
        attackInput = value.isPressed;
    }
}
