using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackWerewolfVisual : MonoBehaviour
{

    [SerializeField] private EnemyAI _enemyAI;
    private Animator _animator;
    private const string IS_RUNNING = "IsRoaming";
    private const string Chasing_Speed_Multiplier = "ChasingSpeedMultiplier";

    private void Awake()
    {
        _animator = GetComponent<Animator>();

    }

    private void Update()
    {
        _animator.SetBool(IS_RUNNING, _enemyAI.IsRunning);
        _animator.SetFloat(Chasing_Speed_Multiplier, _enemyAI.GetRoamingAnimationSpeed());
    }
}
