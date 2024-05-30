using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    public static PlayerVisual Instance { get; private set; }

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private const string IS_RUNNING = "IsRunning";
    private const string ATTACK = "Attack";
    private const string TAKE_DAMAGE = "TakeHit";
    private const string DEATH = "IsDead";

    private void Awake()
    {
        Instance = this;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Player.Instance != null)
        {
            animator.SetBool(IS_RUNNING, Player.Instance.IsRunning());
            AdjustPlayerFacingDirection();
        }
    }

    private void AdjustPlayerFacingDirection()
    {
        if (Player.Instance != null)
        {
            Vector3 mousePos = GameInput.Instance.GetMousePosition();
            Vector3 playerPos = Player.Instance.GetPlayerScreenPosition();

            if (mousePos.x < playerPos.x)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
        }
    }

    public void TriggerAttackAnimation()
    {
        animator.SetTrigger(ATTACK);
    }

    public void TriggerTakeDamageAnimation()
    {
        animator.SetTrigger(TAKE_DAMAGE);
    }

    public void TriggerDeathAnimation()
    {
        animator.SetTrigger(DEATH);
    }
}
