using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[SelectionBase]

public class Player : MonoBehaviour
{
    public static Player Instance {  get; private set; }
    public event EventHandler OnAttack;

    [SerializeField] private float movingSpeed = 3.5f;
    Vector2 inputVector;

    private Rigidbody2D rb;

    private float minMovingSpeed = 0.1f;
    private bool isRunning = false;



    private void Update()
    {
        inputVector = GameInput.Instance.GetMovementVector();
    }

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
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
