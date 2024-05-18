using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class BobAI : MonoBehaviour
{
    Transform player;
    NavMeshAgent agent;
    Animator anim;
    GameManager gm;

    [SerializeField] GameObject exclamationMotif;

    [Header("Pursue Stance")]
    [SerializeField] float maxPursuitDistance;
    [SerializeField] float minPursuitDistance;

    [Header("Attack Stance")]
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

    public enum StanceSelector //Contains enumerated variables for more readable code
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
        switch (currentStance) //Switches which "Stance" is used depending on what the pointer has selected
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


    void IdleStance() //Waits for player to enter room/start combat
    {
        if (!isSwapping && GameManager.hasStartedCombat)
        {
            StartCoroutine(CombatStart());
        }
    }

    void PursueStance() //Maintains perfect distance from player whilst checking regularly if it should attack
    {
        IntervalTimer();

        float distanceFromPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceFromPlayer >= maxPursuitDistance) //Too far from player
        {
            agent.isStopped = false;
            readyToAttack = false;

            agent.SetDestination(player.position);
        }

        else if (distanceFromPlayer <= minPursuitDistance) //Too close to player
        {
            agent.isStopped = false;
            readyToAttack = false;

            Vector3 direction = (player.transform.position - transform.position).normalized;
            agent.SetDestination(transform.position + direction * -5);
        }

        else //Goldilocks
        {
            agent.isStopped = true;
            readyToAttack = true;

            agent.ResetPath();
        }
    }

    void AttackStance()
    {
        if (!isAttacking) //If not already attacking, approach player
        {
            agent.SetDestination(player.position);
            agent.isStopped = false;
        }

        if (Vector3.Distance(player.position, transform.position) < meleeRange && !isAttacking) //When in melee range, attack player
        {
            StartCoroutine(Attack());
        }

        else if (Vector3.Distance(player.position, transform.position) > maxAttackDistance) //If player is too far away, go back to neutral
        {
            currentStance = StanceSelector.Pursuit;
        }
    }

    void RetreatStance()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < retreatDistance) //When the player is too close, run away
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            agent.SetDestination(transform.position + direction * -5);
        }

        retreatTimer -= Time.deltaTime;

        if (retreatTimer <= 0) //When the timer is over, switch back to neutral
        {
            retreatTimer = retreatTimerMax;

            currentStance = StanceSelector.Pursuit;
        }
    }

    void HurtStance()
    {
        //Currently empty, acts as a null state so the enemy is not fighting with it's damage sequence
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

    void AttackCheck() //Rolls a chance for the enemy to attack at set intervals if they are currently ready
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

        yield return new WaitForSeconds(1.5f); //Waits until attack anim is over

        agent.isStopped = false;

        isAttacking = false;
        currentStance = StanceSelector.Retreat;
    }

    public IEnumerator CombatStart()
    {
        isSwapping = true;

        Instantiate(exclamationMotif, new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z), Quaternion.identity);

        yield return new WaitForSeconds(1); //Magic number to keep inspector cleaner :)

        isSwapping = false;

        currentStance = StanceSelector.Pursuit;
    }
}
