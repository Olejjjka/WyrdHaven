using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Wyrd.Utils;

public class BossController : MonoBehaviour
{
    public event System.EventHandler OnEnemyAttack1;
    public event System.EventHandler OnEnemyAttack2;
    public event System.EventHandler OnEnemyAttack3;
    public event System.EventHandler OnEnemyTakeDamage;
    public event System.EventHandler OnEnemyDeath;

    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    [SerializeField] private float meleeDamage1 = 10f;
    [SerializeField] private float meleeDamage2 = 20f;
    [SerializeField] private float meleeDamage3 = 20f;
    [SerializeField] private float meleeAttackCooldown = 2f;
    private float lastMeleeAttackTime = 0f;

    [SerializeField] private float chaseRange = 5f;
    [SerializeField] private float attackRange1 = 1.5f;
    [SerializeField] private float attackRange2 = 2.5f;
    [SerializeField] private float attackRange3 = 5f;

    private NavMeshAgent navMeshAgent;
    private State state;
    private float roamingTime;
    private Vector3 roamPosition;
    private Vector3 startingPosition;
    private float speed = 1.5f;

    private enum State
    {
        Idle,
        Roaming,
        Chasing,
        Attacking1,
        Attacking2,
        Attacking3,
        TakingDamage,
        Death
    }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        state = State.Idle;
        currentHealth = maxHealth;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                IdleBehavior();
                break;
            case State.Roaming:
                RoamingBehavior();
                break;
            case State.Chasing:
                ChasingBehavior();
                break;
            case State.Attacking1:
                AttackingBehavior1();
                break;
            case State.Attacking2:
                AttackingBehavior2();
                break;
            case State.Attacking3:
                AttackingBehavior3();
                break;
            case State.TakingDamage:
                // Handle Taking Damage state
                break;
            case State.Death:
                break;
        }

        CheckCurrentState();
    }

    private void IdleBehavior() { }

    private void RoamingBehavior()
    {
        roamingTime -= Time.deltaTime;
        if (roamingTime < 0)
        {
            Roaming();
            roamingTime = Random.Range(1f, 4f);
        }
    }

    private void ChasingBehavior()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);
        if (distanceToPlayer <= attackRange1)
        {
            navMeshAgent.ResetPath();
        }
        else
        {
            Vector3 playerPosition = Player.Instance.transform.position;
            ChangeFacingDirection(transform.position, playerPosition);
            navMeshAgent.SetDestination(playerPosition);
        }
    }

    private void AttackingBehavior1()
    {
        if (Time.time >= lastMeleeAttackTime + meleeAttackCooldown)
        {
            Attack1();
        }
    }

    private void AttackingBehavior2()
    {
        if (Time.time >= lastMeleeAttackTime + meleeAttackCooldown)
        {
            Attack2();
        }
    }

    private void AttackingBehavior3()
    {
        if (Time.time >= lastMeleeAttackTime + meleeAttackCooldown)
        {
            Attack3();
        }
    }

    private void Roaming()
    {
        startingPosition = transform.position;
        roamPosition = GetRoamingPosition();
        ChangeFacingDirection(startingPosition, roamPosition);
        navMeshAgent.SetDestination(roamPosition);
    }

    private Vector3 GetRoamingPosition()
    {
        return startingPosition + Utils.GetRandomDir() * Random.Range(3f, 7f);
    }

    private void ChangeFacingDirection(Vector3 sourcePosition, Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - sourcePosition).normalized;
        transform.rotation = direction.x < 0 ? Quaternion.Euler(0, -180, 0) : Quaternion.Euler(0, 0, 0);
    }

    private void Attack1()
    {
        lastMeleeAttackTime = Time.time;
        Player.Instance.TakeDamage(meleeDamage1);
        OnEnemyAttack1?.Invoke(this, System.EventArgs.Empty);
        Debug.Log("Enemy performed melee attack 1");
    }

    private void Attack2()
    {
        lastMeleeAttackTime = Time.time;
        Player.Instance.TakeDamage(meleeDamage2);
        OnEnemyAttack2?.Invoke(this, System.EventArgs.Empty);
        Debug.Log("Enemy performed melee attack 2");
    }

    private void Attack3()
    {
        lastMeleeAttackTime = Time.time;
        Player.Instance.TakeDamage(meleeDamage3);
        OnEnemyAttack3?.Invoke(this, System.EventArgs.Empty);
        Debug.Log("Enemy performed melee attack 3");
    }

    public void TakeDamage(float damage)
    {
        OnEnemyTakeDamage?.Invoke(this, System.EventArgs.Empty);
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            state = State.TakingDamage;
            StartCoroutine(RecoverFromDamage());
        }
    }

    private IEnumerator RecoverFromDamage()
    {
        yield return new WaitForSeconds(1f); // Adjust this time as needed for the damage animation
        state = State.Chasing; // Or whatever state you want to transition to after taking damage
    }

    private void Die()
    {
        state = State.Death;
        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;

        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        OnEnemyDeath?.Invoke(this, System.EventArgs.Empty);
        Debug.Log("Enemy died");

        StartCoroutine(DelayedDestroy(10f));
    }

    private IEnumerator DelayedDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private void CheckCurrentState()
    {
        if (state == State.Death) return;

        float distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);
        State newState = state;

        if (distanceToPlayer <= attackRange1 && Time.time >= lastMeleeAttackTime + meleeAttackCooldown)
        {
            newState = State.Attacking1;
        }
        else if (distanceToPlayer <= attackRange2 && Time.time >= lastMeleeAttackTime + meleeAttackCooldown)
        {
            newState = State.Attacking2;
        }
        else if (distanceToPlayer <= attackRange3 && Time.time >= lastMeleeAttackTime + meleeAttackCooldown)
        {
            newState = State.Attacking3;
        }
        else if (distanceToPlayer <= chaseRange)
        {
            newState = State.Chasing;
        }
        else if (state != State.Roaming)
        {
            newState = State.Roaming;
        }

        if (newState != state)
        {
            state = newState;

            if (newState == State.Roaming)
            {
                roamingTime = 0f;
                navMeshAgent.speed = speed;
            }
            else if (newState == State.Chasing)
            {
                navMeshAgent.speed = speed * 1.5f;
            }
            else if (newState == State.Attacking1 || newState == State.Attacking2 || newState == State.Attacking3)
            {
                navMeshAgent.ResetPath();
            }
        }
    }

    public bool IsRunning()
    {
        return state == State.Roaming || state == State.Chasing;
    }

    public float GetRoamingAnimationSpeed()
    {
        return state == State.Chasing ? 1.5f : 1.0f;
    }

    public bool IsAttacking1()
    {
        return state == State.Attacking1;
    }

    public bool IsAttacking2()
    {
        return state == State.Attacking2;
    }

    public bool IsAttacking3()
    {
        return state == State.Attacking3;
    }

    public bool IsTakingDamage()
    {
        return state == State.TakingDamage;
    }

    public bool IsInRangeToAttack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);
        return distanceToPlayer <= attackRange1;
    }

    public bool IsDead()
    {
        return state == State.Death;
    }
}

