using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobAI : MonoBehaviour
{
    public enum StanceSelector
    {
        Idle,
        Pursuit,
        Attack
    }

    public StanceSelector currentStance;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        { 
            currentStance = StanceSelector.Idle;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentStance = StanceSelector.Pursuit;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentStance = StanceSelector.Attack;
        }

        switch (currentStance)
        {
            case StanceSelector.Idle:
                IdleStance();
                break;
            case StanceSelector.Pursuit:
                PursueStance();
                break;
            case StanceSelector.Attack:
                AttackStance();
                break;
        }
    }

    void IdleStance()
    {
        print("Idle");
    }

    void PursueStance()
    {
        print("Pursuing");
    }

    void AttackStance()
    {
        print("Attacking");
    }
}
