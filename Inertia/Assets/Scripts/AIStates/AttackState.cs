using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    bool isAttacking = false;

    public void OnEnter(StateController controller)
    {
        
    }

    public void UpdateState(StateController controller)
    {
        //Encircle the player

        //If too many enemies are attacking the player, cancel the attack;
        if (BobController.enemiesAttacking > BobController.enemyAttackingLimit || isAttacking)
            return;

        controller.StartCoroutine("AttackSequence");
        isAttacking = true;
        BobController.enemiesAttacking++;
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
