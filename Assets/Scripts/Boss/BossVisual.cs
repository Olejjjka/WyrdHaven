using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BossVisual : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private BossController bossController;

    private const string IS_RUNNING = "IsRoaming";
    private const string ATTACK1 = "Attack1";
    private const string ATTACK2 = "Attack2";
    private const string ATTACK3 = "Attack3";
    private const string DAMAGE = "TakingDamage";
    private const string DEATH = "IsDead";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (bossController != null)
        {
            bossController.OnEnemyAttack1 += Boss_Attack1Animation;
            bossController.OnEnemyAttack2 += Boss_Attack2Animation;
            bossController.OnEnemyAttack3 += Boss_Attack3Animation;
            bossController.OnEnemyDeath += Boss_DeathAnimation;
            bossController.OnEnemyTakeDamage += Boss_DamageAnimation;
        }
        else
        {
            Debug.LogError("BossController component missing from the GameObject.");
        }
    }

    private void OnDestroy()
    {
        if (bossController != null)
        {
            bossController.OnEnemyAttack1 -= Boss_Attack1Animation;
            bossController.OnEnemyAttack2 -= Boss_Attack2Animation;
            bossController.OnEnemyAttack3 -= Boss_Attack3Animation;
            bossController.OnEnemyDeath -= Boss_DeathAnimation;
            bossController.OnEnemyTakeDamage -= Boss_DamageAnimation;
        }
    }

    private void Update()
    {
        if (bossController != null)
        {
            bool isRunning = bossController.IsRunning() && !bossController.IsInRangeToAttack() && !bossController.IsDead();
            animator.SetBool(IS_RUNNING, isRunning);
        }
        else
        {
            Debug.LogError("BossController component missing from the GameObject.");
        }
    }

    private void Boss_Attack1Animation(object sender, System.EventArgs e)
    {
        animator.SetTrigger(ATTACK1);
    }

    private void Boss_Attack2Animation(object sender, System.EventArgs e)
    {
        animator.SetTrigger(ATTACK2);
    }

    private void Boss_Attack3Animation(object sender, System.EventArgs e)
    {
        animator.SetTrigger(ATTACK3);
    }

    private void Boss_DamageAnimation(object sender, System.EventArgs e)
    {
        animator.SetTrigger(DAMAGE);
    }

    private void Boss_DeathAnimation(object sender, System.EventArgs e)
    {
        animator.SetTrigger(DEATH);
    }
}
