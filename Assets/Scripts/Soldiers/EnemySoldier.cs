using System.Collections.Generic;
using UnityEngine;
using Enums;

public class EnemySoldier : Soldier
{
    protected void Start()
    {
        FightStage.fightStart.AddListener(_fightStart);
        GameManager.gameOver.AddListener(() => SwitchState(State.Victory));
    }

    private void Update()
    {
        if (_state == State.Idle)
        {
            _target = FindClosestTarget();

            if (_target != null)
            {
                if (_target.position.z > transform.position.z)
                {
                    _target = null;
                    return;
                }

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
                
                if (sold.isDead == true) return;

                sold.Dead(false);
                Dead(false);
            }
        }
    }

    private new void OnDestroy()
    {
        base.OnDestroy();
    }

    protected override Transform FindClosestTarget()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(3f, 1f, _findRange), Quaternion.identity, _friendlyLayer);
        Transform target = null;
        float nearestDistanceToTarget = float.MaxValue;
        float distanceToTarget;

        foreach (Collider col in colliders)
        {
            if (col.tag == "Dead") continue;

            distanceToTarget = Vector3.Distance(transform.position, col.transform.position);

            if (distanceToTarget < nearestDistanceToTarget && col.GetComponent<Soldier>().State != State.Charge)
            {
                nearestDistanceToTarget = distanceToTarget;
                target = col.transform;
            }
        }

        return target;
    }
}