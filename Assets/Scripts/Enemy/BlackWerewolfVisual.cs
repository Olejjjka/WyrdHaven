using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]

public class BlackWerewolfVisual : MonoBehaviour
{

    [SerializeField] private EnemyAI _enemyAI;
    [SerializeField] private EnemyEntity _enemyEntity;

    private Animator _animator;
    private const string IS_RUNNING = "IsRoaming";
    private const string Chasing_Speed_Multiplier = "ChasingSpeedMultiplier";
    private const string ATTACK = "Attack";

    private void Awake()
    {
        _animator = GetComponent<Animator>();

    }

    private void Start()
    {
        _enemyAI.OnEnemyAttack += _enemyAI_OnEnemyAttack;
    }

    private void OnDestroy()
    {
        _enemyAI.OnEnemyAttack -= _enemyAI_OnEnemyAttack;
    }

    private void Update()
    {
        _animator.SetBool(IS_RUNNING, _enemyAI.IsRunning);
        _animator.SetFloat(Chasing_Speed_Multiplier, _enemyAI.GetRoamingAnimationSpeed());
    }

    public void TriggerAttackAnimatoinTurnOff()
    {
        _enemyEntity.PolygonColliderTurnOff();
    }

    public void TriggerAttackAnimatoinTurnOn()
    {
        _enemyEntity.PolygonColliderTurnOn();
    }

    private void _enemyAI_OnEnemyAttack(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(ATTACK);
    }
}
