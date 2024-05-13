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
    }

    private void Update()
    {
        ExitAttack();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }

       
    }

    void Attack()
    {
        if (Time.time - lastComboEnd > 0.8f && comboCounter <= combo.Count)
        {
            CancelInvoke("EndCombo");

            if (Time.time - lastClickedTime >= 0.5f)
            {
                anim.runtimeAnimatorController = combo[comboCounter].animatorOV;
                anim.Play("Attack", 0, 0);

                weapon.damage = combo[comboCounter].damage;

                playerMovement.StopPlayer();
                playerMovement.enabled = false;

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
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            playerMovement.enabled = true;
            Physics.IgnoreLayerCollision(6, 7, false);
            Invoke("EndCombo", 1);
        }
    }

    public void EndCombo()
    {
        playerMovement.enabled = true;
        Physics.IgnoreLayerCollision(6, 7, false);
        comboCounter = 0;
        lastComboEnd = Time.time;
    }
}
