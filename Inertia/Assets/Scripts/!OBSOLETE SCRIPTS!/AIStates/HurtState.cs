using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class HurtState : IState
{
    float iFrames = 0.5f;
    float iFrameTimer = 0;

    public void OnEnter(StateController controller)
    {
        controller.isVulnerable = true;
        iFrameTimer = iFrames;

        //Start hurt sequence
    }

    public void UpdateState(StateController controller)
    {
        iFrameTimer -= Time.deltaTime; //May be better to use a coroutine and return WaitForSeconds later but for now this is fine

        if (iFrameTimer <= 0)
        {
            controller.isVulnerable = false;
            controller.ChangeState(controller.retreatState);
        }
    }

    public void OnHurt(StateController controller)
    {
        //Should not be able to be hurt in this state
    }

    public void OnExit(StateController controller)
    {
        //Cancel any animations ect
    }
}
