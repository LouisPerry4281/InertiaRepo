using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    IState currentState;

    public IdleState idleState = new IdleState();
    public WanderState wanderState = new WanderState();
    public AttackState attackState = new AttackState();
    public RetreatState retreatState = new RetreatState();
    public HurtState hurtState = new HurtState();

    private void Start()
    {
        ChangeState(idleState);
    }

    void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState(this);
        }
    }

    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }
        currentState = newState;
        currentState.OnEnter(this);
    }
}

public interface IState
{
    public void OnEnter(StateController controller);

    public void UpdateState(StateController controller);

    public void OnHurt(StateController controller);

    public void OnExit(StateController controller);
}
