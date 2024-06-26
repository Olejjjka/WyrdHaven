﻿using UnityEngine;

[SelectionBase]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

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
        if (Camera.main != null)
        {
            return Camera.main.WorldToScreenPoint(transform.position);
        }
        else
        {
            Debug.LogError("Main camera not found!");
            return Vector3.zero;
        }
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

                Debug.Log("Player attacked Enemy");
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
        Invoke(nameof(HandleDeath), 3f);
    }

    private void HandleDeath()
    {
        GameObject gameOverUIObject = GameObject.FindGameObjectWithTag("GameOverUI");
        if (gameOverUIObject != null)
        {
            GameOverUI gameOverUI = gameOverUIObject.GetComponent<GameOverUI>();
            if (gameOverUI != null)
            {
                gameOverUI.ShowDeathPanel();
            }
            else
            {
                Debug.LogWarning("GameOverUI component not found on the object with tag GameOverUI");
            }
        }
        else
        {
            Debug.LogWarning("No object found with tag GameOverUI");
        }
    }
}