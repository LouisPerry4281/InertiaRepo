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

    public float baseDamage;
    public float attackTimer;

    public float damage;

    public bool attackInput;
    public bool isAttacking;

    private void Start()
    {
        movement = GetComponent<PlayerRigidbodyMovement>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        damage = (baseDamage * movement.currentJuice) + 30; //Calculate damage based on juice (with a linear baseline)

        if (attackInput)
        {
            if (CanAttack() && !isAttacking) //When the player clicks, check if they can attack, and are not already attacking
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

        //Re-enable player and enemy collisions
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
