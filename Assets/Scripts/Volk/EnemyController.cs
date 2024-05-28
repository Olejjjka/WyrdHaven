using UnityEngine;
using UnityEngine.AI;
using Wyrd.Utils;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    [SerializeField] private float damageAmount = 10f;
    [SerializeField] private float attackCooldown = 2f;
    private float lastAttackTime = 0f;

    [SerializeField] private float chaseRange = 5f; // ������ ����������� ������
    [SerializeField] private float attackRange = 1.5f; // ������ �����

    private NavMeshAgent navMeshAgent;
    private State state;
    private float roamingTime;
    private Vector3 roamPosition;
    private Vector3 startingPosition;

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
        state = State.Idle; // ��������� ��������� - Idle
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
                // ��������� � ��������� ������
                break;
        }

        // ����� �������� ��������� �� ������ ���������� �� ������
        CheckCurrentState();
    }

    private void IdleBehavior()
    {
        // ������ ��� ��������� Idle (��������, �������� ����� ������� ��������)
    }

    private void RoamingBehavior()
    {
        roamingTime -= Time.deltaTime;
        if (roamingTime < 0)
        {
            Roaming();
            roamingTime = Random.Range(1f, 4f); // ��������� �������� ��� ��������
        }
    }

    private void ChasingBehavior()
    {
        navMeshAgent.SetDestination(Player.Instance.transform.position);
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
        return startingPosition + Utils.GetRandomDir() * Random.Range(3f, 7f); // ������ ��������
    }

    private void ChangeFacingDirection(Vector3 sourcePosition, Vector3 targetPosition)
    {
        if (sourcePosition.x > targetPosition.x)
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
        Debug.Log("Enemy �������� Player");
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
        Destroy(gameObject);
        Debug.Log("Enemy ����");
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
                navMeshAgent.speed = navMeshAgent.speed; // ����������� ������ �������� ��� ��������
            }
            else if (newState == State.Chasing)
            {
                navMeshAgent.ResetPath();
                navMeshAgent.speed = navMeshAgent.speed * 1.5f; // ����������� ������ �������� ��� ������
            }
            else if (newState == State.Attacking)
            {
                navMeshAgent.ResetPath();
            }
        }
    }
}
