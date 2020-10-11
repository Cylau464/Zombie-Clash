using System.Collections.Generic;
using UnityEngine;
using Enums;

public class EnemySoldier : Soldier
{
    private void Start()
    {
        FightStage.fightStart.AddListener(_fightStart);
        GameManager.gameOver.AddListener(() => SwitchState(State.Victory));
    }

    private new void Update()
    {
        base.Update();

        if (_state == State.Idle)
        {
            _target = FindClosestTarget();

            if (_target != null)
            {
                _target.GetComponent<Soldier>().ChargeToTarget(transform);
                ChargeToTarget(_target);
            }
        }
    }

    private new void FixedUpdate()
    {
        base.FixedUpdate();

        switch (_state)
        {
            case State.Charge:
                Vector3 direction = (_chargeTarget.position - transform.position).normalized;
                _rigidBody.velocity = _chargeSpeed * direction;
                break;
            case State.Victory:
                _rigidBody.velocity = Vector3.zero;
                isVictory = true;
                break;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == Mathf.Log(_friendlyLayer.value, 2))
        {
            if (State == State.Charge)
            {
                Soldier sold = collision.gameObject.GetComponent<Soldier>();
                sold.Dead(false);
                SwitchState(State.Dead);
            }
        }
    }

    private new void OnDestroy()
    {
        base.OnDestroy();

        if (State == State.Fight)
            FightStage.DefenderDied();
    }
}