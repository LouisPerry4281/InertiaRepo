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

    [Header("Attack Stance")]
    [SerializeField] float attackDistance;
    [SerializeField] float maxAttackDistance;
    [SerializeField] float meleeRange;

    float yet;
    const float interval = 1.0f;
    [SerializeField] int chanceToAttack;

    bool readyToAttack;
    bool isAttacking;
    bool isSwapping;

    [Header("Retreat Stance")]
    [SerializeField] float retreatDistance;
    [SerializeField] float retreatTimer;
    float retreatTimerMax;






    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        gm = FindAnyObjectByType<GameManager>();

        retreatTimerMax = retreatTimer;
    }

    public enum StanceSelector
    {
        Idle,
        Pursuit,
        Attack,
        Retreat,
        Hurt
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
            case StanceSelector.Retreat:
                RetreatStance();
                break;
            case StanceSelector.Hurt:
                HurtStance();
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

    void RetreatStance()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < retreatDistance)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            agent.SetDestination(transform.position + direction * -5);
        }

        retreatTimer -= Time.deltaTime;

        if (retreatTimer <= 0)
        {
            retreatTimer = retreatTimerMax;

            currentStance = StanceSelector.Pursuit;
        }
    }

    void HurtStance()
    {

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

        isAttacking = false;
        currentStance = StanceSelector.Retreat;
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
