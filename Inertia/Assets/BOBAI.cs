using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    IState currentState;

    public IdleState idleState = new IdleState(); //They will wait on standby for the player to open the door. Honestly just a temporary state, will probably be transformed into something more useful later down the line
    public WanderState wanderState = new WanderState(); //They will roam around at random
    public AttackState attackState = new AttackState(); //They will approach the player and start an attack
    public RetreatState retreatState = new RetreatState(); //They will actively run away from the player
    public HurtState hurtState = new HurtState(); //They become staggered and lose health

    public bool isVulnerable;

    private void Start()
    {
        ChangeState(idleState);
    }

    void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState(this); //Basically replaces update for individual states
        }
    }

    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this); //Run exit code for the state being transitioned away from
        }
        currentState = newState;
        currentState.OnEnter(this); //Run start code for the state being transitioned to
    }
}

public interface IState
{
    public void OnEnter(StateController controller); //Isn't strictly necessary right now but will prove useful during later implementations

    public void UpdateState(StateController controller);

    public void OnHurt(StateController controller);

    public void OnExit(StateController controller);
}
