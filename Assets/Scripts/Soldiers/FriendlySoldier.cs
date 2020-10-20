using System.Collections.Generic;
using UnityEngine;
using Enums;
using System.Collections;
using UnityEngine.Events;

public class FriendlySoldier : Soldier
{
    [SerializeField] private LayerMask _roadEdgeLayer = 0;
    private bool _scriptIsActive;
    private bool _rightSideBlock;
    private bool _leftSideBlock;

    [Header("Road Edge Check")]
    [SerializeField] private bool _showDebugRays = false;
    [SerializeField] private float _checkDistance = 1f;
    [SerializeField] private float _checkHeight = -.6f;

    private int _defaultMaxHealth;
    private int _defaultDamage;
    private Vector3 _defaultScale;

    private void Start()
    {
        if (gameObject.tag == "Main Soldier")
        {
            _defaultMaxHealth = _maxHealth;
            _defaultDamage = _damage;
            _defaultScale = transform.localScale;
            UpdateStats();
            UpgradeStats.healthUpgrade.AddListener(UpdateStats);
        }

        gameObject.tag = "Attacking";
        _scriptIsActive = true;
        FightStage.fightStart.AddListener(_fightStart);
        FightStage.fightEnd.AddListener(FightEnd);
        GameManager.current.SolidersCount++;
        GameManager.levelCompleted.AddListener(() => SwitchState(State.Victory));
    }

    private new void FixedUpdate()
    {
        base.FixedUpdate();
        CheckRoadEdge();

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
            case State.Victory:
                _rigidBody.velocity = Vector3.zero;
                isVictory = true;
                break;
            case State.Siege:
                MoveToTarget();
                break;
        }
    }

    private void CheckRoadEdge()
    {
        if (_showDebugRays)
        {
            Debug.DrawRay(transform.position + Vector3.up * _checkHeight, Vector3.right * _checkDistance, Color.red);
            Debug.DrawRay(transform.position + Vector3.up * _checkHeight, Vector3.left * _checkDistance, Color.red);
        }

        bool rightCheck = Physics.Raycast(transform.position + Vector3.up * _checkHeight, Vector3.right, _checkDistance, _roadEdgeLayer);
        bool leftCheck = Physics.Raycast(transform.position + Vector3.up * _checkHeight, Vector3.left, _checkDistance, _roadEdgeLayer);

        if(_rightSideBlock != rightCheck)
        {
            _rightSideBlock = rightCheck;
            InputController.rightSideBlock = rightCheck;
        }

        if(_leftSideBlock != leftCheck)
        {
            _leftSideBlock = leftCheck;
            InputController.leftSideBlock = leftCheck;
        }
    }
    private new void OnDestroy()
    {
        base.OnDestroy();
    }

    public override void Dead(bool destroy)
    {
        if (gameObject.tag == "Dead") return;

        gameObject.tag = "Dead";

        if(_rightSideBlock)
            InputController.rightSideBlock = false;
        else if(_leftSideBlock)
            InputController.leftSideBlock = false;

        CameraSwitch.resetTargetEvent.Invoke();

        if (_scriptIsActive)
            GameManager.current.SolidersCount--;

        if (destroy)
        {
            Instantiate(_destroyParticle, transform.position, Quaternion.identity);
            AudioManager.PlayClipAtPosition(_destroyClip, transform.position, 1f, 1f, Random.Range(.5f, 1f));
            Destroy(gameObject);
        }
        else
        {
            SwitchState(State.Dead);
            AudioManager.PlayClipAtPosition(_deadClip, transform.position);
            Destroy(gameObject, 5f);
        }
    }

    private void UpdateStats()
    {
        _damage = _defaultDamage + UpgradeStats.damageMultiplier / 2;
        _maxHealth += _defaultMaxHealth + UpgradeStats.healthMultiplier;
        _health = _maxHealth;
        transform.localScale = _defaultScale + Vector3.one * UpgradeStats.healthScaleIncreaser * (UpgradeStats.healthLevel - 1);
    }

    private void FightEnd()
    {
        _target = CastleSiege.moveTarget;
        SwitchState(State.Siege);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 13) //13 - castle layer
        {
            SwitchState(State.Victory);
        }
    }
}