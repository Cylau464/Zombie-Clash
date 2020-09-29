using System.Collections.Generic;
using UnityEngine;
using Enums;

public class EnemySoldier : Soldier
{
    [SerializeField] private float _checkRange = 15f;

    private float nearestDistance = float.MaxValue;
    private float distance;

    private void Update()
    {
        if (_state == State.Idle)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _checkRange, _friendlyLayer);
            Transform target = null;

            foreach (Collider col in colliders)
            {
                distance = Vector3.Distance(transform.position, col.transform.position);

                if (distance < nearestDistance && col.GetComponent<Soldier>().State != State.Charge)
                {
                    nearestDistance = distance;
                    target = col.transform;
                }
            }

            if (target != null)
            {
                target.GetComponent<Soldier>().ChargeToTarget(transform);
                ChargeToTarget(target.transform);
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

    private void OnDestroy()
    {
        if (State == State.Fight)
            FightStage.DefenderDied();
    }
}