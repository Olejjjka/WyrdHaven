using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[SelectionBase]

public class Player : MonoBehaviour
{
    public static Player Instance {  get; private set; }
    public event EventHandler OnAttack;

    [SerializeField] private int _damageAmount = 2;
    [SerializeField] private float movingSpeed = 3.5f;
    Vector2 inputVector;

    private Rigidbody2D rb;

    private float minMovingSpeed = 0.1f;
    private bool isRunning = false;

    private PolygonCollider2D _polygonCollider2D;



    private void Update()
    {
        inputVector = GameInput.Instance.GetMovementVector();
    }

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
    }

    private void Start()
    {

        GameInput.Instance.OnPlayerAttack += GameInput_OnPlayerAttack;

    }

    private void GameInput_OnPlayerAttack(object sender, System.EventArgs e)
    {

        Attack();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + inputVector * (movingSpeed * Time.fixedDeltaTime));

        if (Mathf.Abs(inputVector.x) > minMovingSpeed || Mathf.Abs(inputVector.y) > minMovingSpeed)
            isRunning = true;
        else
            isRunning = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out EnemyEntity enemyEntity))
        {
            enemyEntity.TakeDamage(_damageAmount);
        }
    }

    public void AttackColliderTurnOff()
    {
        _polygonCollider2D.enabled = false;
    }

    public void AttackColliderTurnOn()
    {
        _polygonCollider2D.enabled = true;
    }

    private void Attack()
    {
        OnAttack?.Invoke(this, EventArgs.Empty);
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


}
