using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    public void OnEnter(StateController controller)
    {

    }

    public void UpdateState(StateController controller)
    {
        //When player is in LOS, switch to attack state
    }

    public void OnHurt(StateController controller)
    {

    }

    public void OnExit(StateController controller)
    {

    }
}
