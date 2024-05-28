using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private float movingSpeed = 3.5f;
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    [SerializeField] private float attackRange = 1.5f; // ������ ����� �����
    [SerializeField] private float attackDamage = 25f; // ���� ����� �����

    private Rigidbody2D rb;
    private bool isRunning = false;

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        // ��������� �����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Attack key pressed");
            Attack();
        }
    }

    private void FixedUpdate()
    {
        Vector2 inputVector = GetMovementInput();
        rb.MovePosition(rb.position + inputVector * (movingSpeed * Time.fixedDeltaTime));

        if (Mathf.Abs(inputVector.x) > 0.1f || Mathf.Abs(inputVector.y) > 0.1f)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }

    public bool IsRunning()
    {
        return isRunning;
    }

    public Vector3 GetPlayerScreenPosition()
    {
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return playerScreenPosition;
    }

    private Vector2 GetMovementInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        return new Vector2(horizontalInput, verticalInput);
    }

    private void Attack()
    {
        // ������� ���� ������ � ������� � ������� �� ����
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);

        foreach (Collider2D collider in colliders)
        {
            // ���������, �������� �� ��������� BoxCollider2D
            if (collider is BoxCollider2D && collider.CompareTag("Enemy1"))
            {
                collider.GetComponent<EnemyController>().TakeDamage(attackDamage);
                Debug.Log("Player attacked Enemy with BoxCollider2D");
            }
        }
    }


    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Current health: " + currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // ���������� ����������� �������� ��� ������ �����, ��������, �������� ������ ��� ���������� ������.
        Debug.Log("Player died!");
    }
}
