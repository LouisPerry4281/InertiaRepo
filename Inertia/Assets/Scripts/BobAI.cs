using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BobAI : MonoBehaviour
{
    Transform player;
    NavMeshAgent agent;

    [SerializeField] float attackDistance;
    [SerializeField] float meleeRange;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
    }

    public enum StanceSelector
    {
        Idle,
        Pursuit,
        Attack
    }

    public StanceSelector currentStance;

    private void Update()
    {
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
        agent.SetDestination(player.position);

        if (Vector3.Distance(player.position, transform.position) < attackDistance)
        {
            currentStance = StanceSelector.Attack;
        }
    }

    void AttackStance()
    {
        agent.SetDestination(player.position);

        if (Vector3.Distance(player.position, transform.position) < meleeRange)
        {
            print("Swing");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
}
