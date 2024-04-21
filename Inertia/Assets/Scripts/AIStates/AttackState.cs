using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    public void OnEnter(StateController controller)
    {
        
    }

    public void UpdateState(StateController controller)
    {
        //Encircle the player

        //Grab the amount of enemies currently attacking the player
        //If amount of enemies attacking is lower than the lower limit, start attack sequence

        //AttackSequence
        //Number of enemies attacking++
        //Approach player
        //When within range, attack
        //Switch to retreat state
    }

    public void OnHurt(StateController controller)
    {
        //Cancel attack (if animation < 50% complate)
        //Switch to hurt state
    }

    public void OnExit(StateController controller)
    {
        //If currently attacking, number of enemies attacking-- (This will occur regardless of if the attack finishes or not)
    }
}
