using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOBAI : MonoBehaviour
{
    enum States
    {
        Idle,
        Wander,
        Attack,
        Retreat
    }

    States currentState;

    private void Start()
    {
        currentState = States.Idle;
    }

    private void Update()
    {
        StateSwapper();
    }

    private void StateSwapper()
    {
        switch (currentState)
        {
            case States.Idle:
                currentState = States.Idle;
                break;
            case States.Wander:
                currentState = States.Wander;
                break;
            case States.Attack:
                currentState = States.Attack;
                break;
            case States.Retreat:
                currentState = States.Retreat;
                break;
        }
    }
}
