using System;
using UnityEngine;

[SelectionBase]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    //public event Action OnPlayerDeath; // Событие смерти игрока

    [SerializeField] private float movingSpeed = 3.5f;
    [SerializeField] private float maxHealth = 25f;
    private float currentHealth;

    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackDamage = 25f;

    private Rigidbody2D rb;
    private bool isRunning = false;
    private bool isDead = false;

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (isDead || PauseMenu.GameIsPaused) return;

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Left mouse button pressed");
            Attack();
        }
    }

    private void FixedUpdate()
    {
        if (isDead || PauseMenu.GameIsPaused) return;

        Vector2 inputVector = GetMovementInput();
        rb.MovePosition(rb.position + inputVector * (movingSpeed * Time.fixedDeltaTime));

        isRunning = inputVector.magnitude > 0.1f;
    }

    public bool IsRunning()
    {
        return isRunning;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }


    public Vector3 GetPlayerScreenPosition()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    private Vector2 GetMovementInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        return new Vector2(horizontalInput, verticalInput);
    }

    private void Attack()
    {
        PlayerVisual.Instance.TriggerAttackAnimation();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);

        foreach (Collider2D collider in colliders)
        {
            if (collider is BoxCollider2D && collider.CompareTag("Enemy1"))
            {
                if (collider.GetComponent<EnemyController>())
                    collider.GetComponent<EnemyController>().TakeDamage(attackDamage);
                if (collider.GetComponent<BossController>())
                    collider.GetComponent<BossController>().TakeDamage(attackDamage);

                Debug.Log("Player attacked Enemy with BoxCollider2D");
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log("Current health: " + currentHealth);
        PlayerVisual.Instance.TriggerTakeDamageAnimation();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        PlayerVisual.Instance.TriggerDeathAnimation();
        Debug.Log("Player died!");

        // Вызываем событие смерти игрока, если есть подписчики
        //OnPlayerDeath?.Invoke();
    }
}
