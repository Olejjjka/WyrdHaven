using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class VolkVisual : MonoBehaviour
{
    [SerializeField] private EnemyController _enemyController;

    private Animator _animator;
    private const string IS_RUNNING = "IsRoaming";
    private const string CHASING_SPEED_MULTIPLIER = "ChasingSpeedMultiplier";
    private const string ATTACK = "Attack";
    private const string DEATH = "IsDead";


    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (_enemyController != null)
        {
            _enemyController.OnEnemyAttack += _enemyAI_OnEnemyAttack;
            _enemyController.OnEnemyDeath += _enemyAI_OnEnemyDeath;
        }
        else
        {
            Debug.LogError("EnemyController component missing from the GameObject.");
        }
    }

    private void OnDestroy()
    {
        if (_enemyController != null)
        {
            _enemyController.OnEnemyAttack -= _enemyAI_OnEnemyAttack;
            _enemyController.OnEnemyDeath -= _enemyAI_OnEnemyDeath;
        }
    }

    private void Update()
    {
        if (_enemyController != null)
        {
            bool isRunning = _enemyController.IsRunning() && !_enemyController.IsInRangeToAttack() && !_enemyController.IsDead();
            _animator.SetBool(IS_RUNNING, isRunning);
            _animator.SetFloat(CHASING_SPEED_MULTIPLIER, _enemyController.GetRoamingAnimationSpeed());
        }
        else
        {
            Debug.LogError("EnemyController component missing from the GameObject.");
        }
    }

    private void _enemyAI_OnEnemyAttack(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(ATTACK);
    }

    private void _enemyAI_OnEnemyDeath(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(DEATH);
    }
}
