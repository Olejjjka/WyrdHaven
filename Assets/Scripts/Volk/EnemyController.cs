using UnityEngine;
using UnityEngine.AI;
using Wyrd.Utils;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public event System.EventHandler OnEnemyAttack;
    public event System.EventHandler OnEnemyDeath;

    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    [SerializeField] private float damageAmount = 10f;
    [SerializeField] private float attackCooldown = 2f;
    private float lastAttackTime = 0f;

    [SerializeField] private float chaseRange = 5f; // Distance to start chasing
    [SerializeField] private float attackRange = 1.5f; // Distance to start attacking

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
        Attacking,
        Death
    }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        state = State.Idle; // Initial state
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
            case State.Attacking:
                AttackingBehavior();
                break;
            case State.Death:
                // Handle death state
                break;
        }

        // Check current state based on distance to player
        CheckCurrentState();
    }

    private void IdleBehavior()
    {
        // Logic for Idle state
    }

    private void RoamingBehavior()
    {
        roamingTime -= Time.deltaTime;
        if (roamingTime < 0)
        {
            Roaming();
            roamingTime = Random.Range(1f, 4f); // Set next roaming time
        }
    }

    private void ChasingBehavior()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);
        if (distanceToPlayer <= attackRange)
        {
            // Остановить движение и анимацию, если в пределах дистанции атаки
            navMeshAgent.ResetPath();
        }
        else
        {
            // Продолжить преследование
            Vector3 playerPosition = Player.Instance.transform.position;
            ChangeFacingDirection(transform.position, playerPosition);
            navMeshAgent.SetDestination(playerPosition);
        }
    }

    private void AttackingBehavior()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
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
        return startingPosition + Utils.GetRandomDir() * Random.Range(3f, 7f); // Generate random position
    }

    private void ChangeFacingDirection(Vector3 sourcePosition, Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - sourcePosition).normalized;
        if (direction.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void Attack()
    {
        lastAttackTime = Time.time;
        Player.Instance.TakeDamage(damageAmount);
        OnEnemyAttack?.Invoke(this, System.EventArgs.Empty); // Trigger attack event
        Debug.Log("Enemy attacked Player");
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
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

        OnEnemyDeath?.Invoke(this, System.EventArgs.Empty); // Триггер события смерти
        Debug.Log("Enemy died");

        // Запустить задержку перед уничтожением объекта
        StartCoroutine(DelayedDestroy(2f)); // 2 секунды задержки перед уничтожением
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

        if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            newState = State.Attacking;
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
                navMeshAgent.speed = speed; // Reset speed for roaming
            }
            else if (newState == State.Chasing)
            {
                navMeshAgent.speed = speed * 1.5f; // Increase speed for chasing
            }
            else if (newState == State.Attacking)
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

    public bool IsAttacking()
    {
        return state == State.Attacking;
    }

    public bool IsInRangeToAttack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);
        return distanceToPlayer <= attackRange;
    }

    public bool IsDead()
    {
        return state == State.Death;

    }

}
