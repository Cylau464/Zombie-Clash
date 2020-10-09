using System.Collections.Generic;
using UnityEngine;
using Enums;
using System.Collections;
using UnityEngine.Events;

public class FriendlySoldier : Soldier
{
    [SerializeField] private LayerMask _roadEdgeLayer;
    private bool _scriptIsActive;
    private bool _rightSideBlock;
    private bool _leftSideBlock;

    private void Start()
    {
        _damage = UpgradeStats.damage;
        _maxHealth = UpgradeStats.health;
        _scriptIsActive = true;
        FightStage.fightStart.AddListener(_fightStart);
        GameManager.current.SolidersCount++;
        GameManager.levelCompleted.AddListener(() => SwitchState(State.Idle));
    }

    private new void FixedUpdate()
    {
        base.FixedUpdate();

        switch (_state)
        {
            case State.Run:
            case State.Idle:
                Vector3 speed = Vector3.Scale(InputController.moveDirection, new Vector3(_sideSpeed, 0f, _moveSpeed)) * InputController.acceleration;
                speed.y = _rigidBody.velocity.y;
                _rigidBody.velocity = speed;
                break;
            case State.Charge:
                Vector3 direction = (_chargeTarget.position - transform.position).normalized;
                _rigidBody.velocity = _chargeSpeed * direction;
                break;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == Mathf.Log(_roadEdgeLayer.value, 2))
        {
            // Right side
            if (collision.transform.position.x > transform.position.x)
            {
                InputController.rightSideBlock = true;
                _rightSideBlock = true;
            }
            // Left side
            else if (collision.transform.position.x < transform.position.x)
            {
                InputController.leftSideBlock = true;
                _leftSideBlock = true;
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.layer == Mathf.Log(_roadEdgeLayer.value, 2))
        {
            // Right side
            if (collision.transform.position.x > transform.position.x)
            {
                InputController.rightSideBlock = false;
                _rightSideBlock = false;
            }
            // Left side
            else if (collision.transform.position.x < transform.position.x)
            {
                InputController.leftSideBlock = false;
                _leftSideBlock = false;
            }
        }
    }

    private new void OnDestroy()
    {
        base.OnDestroy();
    }

    public override IEnumerator DestroySelf()
    {
        transform.position = new Vector3(-100f, 0f, -100f);
        gameObject.tag = "Dead";

        if(_rightSideBlock)
            InputController.rightSideBlock = false;
        else if(_leftSideBlock)
            InputController.leftSideBlock = false;

        if (isCameraTarget)
            CameraSwitch.resetTargetEvent.Invoke();

        if (_scriptIsActive)
            GameManager.current.SolidersCount--;

        yield return new WaitForFixedUpdate();
        Destroy(gameObject);
    }
}