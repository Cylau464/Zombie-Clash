﻿using System.Collections.Generic;
using UnityEngine;
using Enums;

public class EnemySoldier : Soldier
{
    private void Start()
    {
        FightStage.fightStart.AddListener(_fightStart);
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == Mathf.Log(_friendlyLayer.value, 2))
        {
            if (State == State.Charge)
            {
                Destroy(collision.gameObject);
                Destroy(gameObject);
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