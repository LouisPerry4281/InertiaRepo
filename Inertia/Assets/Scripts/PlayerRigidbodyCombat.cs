using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerRigidbodyCombat : MonoBehaviour
{
    PlayerRigidbodyMovement movement;
    Animator anim;

    [SerializeField] Collider hitBox;

    public float damage;
    public float attackTimer;

    public bool attackInput;
    public bool isAttacking;

    private void Start()
    {
        movement = GetComponent<PlayerRigidbodyMovement>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (attackInput)
        {
            if (CanAttack() && !isAttacking)
            {
                StartCoroutine(Attack());
            }
        }

        else
            attackInput = false;
    }

    IEnumerator Attack()
    {
        attackInput = false;
        isAttacking = true;

        //Disables player and enemy collisions
        Physics.IgnoreLayerCollision(6, 7, true);

        hitBox.enabled = true;

        anim.Play("Attack");

        yield return new WaitForSeconds(attackTimer);

        Physics.IgnoreLayerCollision(6, 7, false);

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
