using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;

public class AttackState : IState
{
    float attackDistance = 1.5f;

    NavMeshAgent agent;
    Transform player;

    bool isAttacking = false;

    public void OnEnter(StateController controller)
    {
        agent = controller.GetComponent<NavMeshAgent>();
        player = GameObject.FindAnyObjectByType<PlayerRigidbodyMovement>().GetComponent<Transform>();
    }

    public void UpdateState(StateController controller)
    {
        //Encircle the player

        //If too many enemies are attacking the player, cancel the attack;
        if (BobController.enemiesAttacking > BobController.enemyAttackingLimit || isAttacking)
            return;

        controller.StartCoroutine(this.AttackSequence(controller)); //Coroutines require monobehaviour, this is a workaround allowing the controller script to be the trigger of the coroutine on this script
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

    IEnumerator AttackSequence(StateController controller)
    {
        Debug.Log("StartingAttack");

        while (!DistanceCheck(controller))
        {
            Debug.Log("Looping");
            agent.SetDestination(player.position);
            yield return new WaitForSeconds(0.2f); //Prevents it becoming too performant
        }

        agent.isStopped = true;
        //Attack

        //Wait for attack finish

        controller.ChangeState(controller.retreatState);
    }

    bool DistanceCheck(StateController controller)
    {
        if (Vector3.Distance(controller.transform.position, player.position) < attackDistance)
        {
            return true;
        }

        return false;
    }
}
