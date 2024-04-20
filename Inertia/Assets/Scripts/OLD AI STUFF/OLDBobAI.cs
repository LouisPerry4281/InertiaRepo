using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class OLDBobAI : MonoBehaviour
{
    Transform player;
    NavMeshAgent agent;
    Animator anim;
    GameManager gm;

    [SerializeField] GameObject exclamationMotif;

    [SerializeField] float attackDistance;
    [SerializeField] float maxAttackDistance;
    [SerializeField] float meleeRange;

    bool readyToAttack;
    bool isAttacking;
    bool isSwapping;

    float yet;
    const float interval = 1.0f;
    [SerializeField] int chanceToAttack;


    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        gm = FindAnyObjectByType<GameManager>();
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
        if (!isSwapping && GameManager.hasStartedCombat)
        {
            StartCoroutine(CombatStart());
        }
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
        if (!isAttacking)
        {
            agent.SetDestination(player.position);
            agent.isStopped = false;
        }

        if (Vector3.Distance(player.position, transform.position) < meleeRange && !isAttacking)
        {
            StartCoroutine(Attack());
        }

        else if (Vector3.Distance(player.position, transform.position) > maxAttackDistance)
        {
            currentStance = StanceSelector.Pursuit;
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

    IEnumerator Attack()
    {
        agent.isStopped = true;
        isAttacking = true;

        anim.Play("Attack");

        yield return new WaitForSeconds(1);

        agent.isStopped = false;
        Vector3 direction = (player.transform.position - transform.position).normalized;
        agent.SetDestination(transform.position + direction * -10);

        yield return new WaitForSeconds(3);

        currentStance = StanceSelector.Pursuit;

        agent.isStopped = false;
        isAttacking = false;
        
    }

    IEnumerator CombatStart()
    {
        isSwapping = true;

        Instantiate(exclamationMotif, new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z), Quaternion.identity);

        yield return new WaitForSeconds(1);

        isSwapping = false;

        currentStance = StanceSelector.Pursuit;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
}
