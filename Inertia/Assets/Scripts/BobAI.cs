using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class BobAI : MonoBehaviour
{
    Transform player;
    NavMeshAgent agent;

    [SerializeField] float attackDistance;
    [SerializeField] float meleeRange;

    public bool readyToAttack;

    float yet;
    const float interval = 1.0f;
    [SerializeField] int chanceToAttack;


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

        IntervalTimer();

        if (Vector3.Distance(player.position, transform.position) <= attackDistance)
        {
            readyToAttack = true;
            agent.isStopped = true;
        }

        else
        {
            readyToAttack= false;
            agent.isStopped = false;
        }
    }

    void AttackStance()
    {
        print("Att");
        agent.SetDestination(player.position);
        agent.isStopped = false;

        if (Vector3.Distance(player.position, transform.position) < meleeRange)
        {
            print("Swing");
        }
    }

    private void IntervalTimer()
    {
        yet += Time.deltaTime;

        if (yet >= interval)
        {
            yet -= interval;

            AttackCheck();
        }
    }

    void AttackCheck()
    {
        int roll = Random.Range(0, chanceToAttack);

        if (roll == chanceToAttack - 1 && readyToAttack)
        {
            readyToAttack = false;
            agent.isStopped = false;
            currentStance = StanceSelector.Attack;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
}
