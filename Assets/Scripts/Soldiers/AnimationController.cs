using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Soldier _soldier = null;
    [SerializeField] private Animator _animator = null;

    private int isAttackingParamID;
    private int isVictoryParamID;
    private int isDeadParamID;
    private int isChargeParamID;
    private int speedParamID;
    private int deadAnimParamID;
    private int attackAnimParamID;

    private void Start()
    {
        isAttackingParamID          = Animator.StringToHash("isAttacking");
        isDeadParamID               = Animator.StringToHash("isDead");
        isVictoryParamID            = Animator.StringToHash("isVictory");
        isChargeParamID             = Animator.StringToHash("isCharge");
        speedParamID                = Animator.StringToHash("speed");
        deadAnimParamID             = Animator.StringToHash("deadAnimation");
        attackAnimParamID           = Animator.StringToHash("attackAnimation");
    }

    void Update()
    {
        _animator.SetBool(isAttackingParamID, _soldier.isAttack);
        _animator.SetBool(isDeadParamID, _soldier.isDead);
        _animator.SetBool(isVictoryParamID, _soldier.isVictory);
        _animator.SetBool(isChargeParamID, _soldier.isCharge);
        
        _animator.SetInteger(deadAnimParamID, _soldier.deadAnimIndex);
        _animator.SetInteger(attackAnimParamID, _soldier.attackAnimIndex);
        
        _animator.SetFloat(speedParamID, _soldier.Speed);
    }

    private void GiveDamage()
    {
        _soldier.GiveDamage();
    }

    private void EndOfAttack()
    {
        _soldier.EndOfAttack();
    }
}
