using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatCombo : MonoBehaviour
{
    public List<AttackSO> combo;
    float lastClickedTime;
    float lastComboEnd;
    int comboCounter;

    [SerializeField] float lungeForce;
    [SerializeField] float specialCost;
    [SerializeField] GameObject specialAttackPrefab;

    [SerializeField] GameObject weaponTrail;

    Animator anim;
    Weapon weapon;
    PlayerRigidbodyMovement playerMovement;
    Rigidbody rb;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        weapon = GetComponentInChildren<Weapon>();
        playerMovement = GetComponent<PlayerRigidbodyMovement>();
        rb = GetComponent<Rigidbody>();
        weaponTrail.SetActive(false);
    }

    private void Update()
    {
        if (PauseMenuManager.isPaused) //If game is paused, do not handle input
        {
            return;
        }

        ExitAttack();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }

       if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            SpecialAttack();
        }
    }

    public void SpecialAttack()
    {
        if (playerMovement.currentJuice < specialCost)
        {
            return;
        }

        Instantiate(specialAttackPrefab, transform.position, Quaternion.identity);
        playerMovement.JuiceChange(-specialCost);
    }

    void Attack()
    {
        if (Time.time - lastComboEnd > 0.8f && comboCounter <= combo.Count)
        {
            CancelInvoke("EndCombo");

            if (Time.time - lastClickedTime >= 0.3f)
            {
                anim.runtimeAnimatorController = combo[comboCounter].animatorOV;
                anim.Play("Attack", 1, 0);

                weapon.damage = combo[comboCounter].damage;

                playerMovement.StopPlayer();
                playerMovement.enabled = false;

                weaponTrail.SetActive(true);

                Physics.IgnoreLayerCollision(6, 7, true);

                rb.AddForce(transform.forward * lungeForce, ForceMode.Impulse);
                //VFX ECT HERE

                comboCounter++;
                lastClickedTime = Time.time;
                
                if (comboCounter + 1 > combo.Count)
                {
                    comboCounter = 0;
                }
            }
        }
    }

    void ExitAttack()
    {
        if (anim.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.9f && anim.GetCurrentAnimatorStateInfo(1).IsTag("Attack"))
        {
            playerMovement.enabled = true;
            Physics.IgnoreLayerCollision(6, 7, false);
            Invoke("EndCombo", 1);

            weaponTrail.SetActive(false);

            CancelInvoke("ExitAttack");
        }
    }

    public void EndCombo()
    {
        playerMovement.enabled = true;
        Physics.IgnoreLayerCollision(6, 7, false);
        comboCounter = 0;
        lastComboEnd = Time.time;

        CancelInvoke("EndCombo");
    }
}
