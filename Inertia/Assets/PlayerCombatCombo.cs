using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatCombo : MonoBehaviour
{
    public List<AttackSO> combo;
    float lastClickedTime;
    float lastComboEnd;
    int comboCounter;

    Animator anim;
    Weapon weapon;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        weapon = GetComponentInChildren<Weapon>();
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
        if (Time.time - lastComboEnd > 2f && comboCounter <= combo.Count)
        {
            CancelInvoke("EndCombo");

            if (Time.time - lastClickedTime >= 0.7f)
            {
                anim.runtimeAnimatorController = combo[comboCounter].animatorOV;
                anim.Play("Attack", 0, 0);

                weapon.damage = combo[comboCounter].damage;

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
            Invoke("EndCombo", 1);
        }
    }

    void EndCombo()
    {
        comboCounter = 0;
        lastComboEnd = Time.time;
    }
}
