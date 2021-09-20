﻿using System.Collections.Generic;
using UnityEngine;
using Enums;
using UnityEngine.Events;
using System.Collections;

public class Soldier : MonoBehaviour
{
    [SerializeField] protected State _state = State.Idle;
    public State State
    {
        get { return _state; }
        protected set { _state = value; }
    }

    [SerializeField] protected SoldierType _type = SoldierType.Neutral;

    [Header("Fight Attributes")]
    [SerializeField] protected int _maxHealth = 1;
    [SerializeField] protected int _health;
    [SerializeField] protected int _damage = 1;
    [SerializeField] private float _attackRange = 1f;
    [SerializeField] private float _attackRange2 = 2f;
    private float _delayAfterAttack = .1f;
    private bool _isAttackDelay;

    [HideInInspector] public int attackAnimIndex = 0;
    [HideInInspector] public int deadAnimIndex = 0;

    [Header("Charge Properties")]
    protected Transform _chargeTarget;

    [Header("Movement Properties")]
    [SerializeField] protected float _moveSpeed = 7f;
    [SerializeField] protected float _sideSpeed = 15f;
    [Space]
    [SerializeField] protected float _chargeSpeed = 30f;
    [SerializeField] private float _rotateSpeed = 5f;
    public float Speed
    {
        get { return _rigidBody.velocity.magnitude; }
    }

    [Header("Materials")]
    [SerializeField] protected Material _neutralMat = null;
    [SerializeField] protected Material _friendlyMat = null;

    [Header("Find Target Properties")]
    [SerializeField] protected float _findRange = 15f;
    [SerializeField] protected float _findWidth = 3f;
    protected Transform _target;

    [Header("Camera Properties")]
    //[HideInInspector] public bool isCameraTarget;

    [Header("References")]
    [SerializeField] private SkinnedMeshRenderer _mesh = null;
    [SerializeField] protected Rigidbody _rigidBody = null;
    [SerializeField] protected LayerMask _friendlyLayer = 0;
    [SerializeField] private LayerMask _enemyLayer = 0;
    [SerializeField] protected FriendlySoldier _friendlyScript = null;

    [Header("Status Flags")]
    public bool isCharge;
    public bool isDead;
    public bool isAttack;
    public bool isVictory;

    [Header("Animations")]
    [SerializeField] private int _fightAnimCount = 2;
    [SerializeField] protected GameObject _destroyParticle = null;

    [SerializeField] protected AudioClip _destroyClip = null;
    [SerializeField] protected AudioClip _deadClip = null;
    [HideInInspector] public bool isStepSoundSource;

    protected UnityAction _fightStart;

    protected void OnEnable()
    {
        _health = _maxHealth;
        _rigidBody = _rigidBody == null ? GetComponent<Rigidbody>() : _rigidBody;
        SwitchType(_type);
        _fightStart = FightStart;
    }

    protected void FixedUpdate()
    {
        if (_state == State.Fight)
        {
            if (_target == null)
            {
                _target = FindClosestTarget();
                _rigidBody.velocity = Mathf.Sign(transform.forward.z) * Vector3.forward * _moveSpeed;
            }
            else if (isAttack == false && _isAttackDelay == false)
            {
                if (Vector3.Distance(transform.position, _target.position) > _attackRange)
                {
                    _target = FindClosestTarget();
                    MoveToTarget();

                    if(Vector3.Distance(transform.position, _target.position) <= _attackRange2 && _rigidBody.velocity.magnitude <= 1f)
                    {
                        _isAttackDelay = true;
                        Invoke(nameof(Attack), _delayAfterAttack);
                    }
                }
                else
                {
                    _isAttackDelay = true;
                    Invoke(nameof(Attack), _delayAfterAttack);
                }
            }
            else
                _rigidBody.velocity = Vector3.zero;
        }

        if (_rigidBody.velocity.magnitude > .1f)
        {
            Quaternion rot = Quaternion.LookRotation(new Vector3(_rigidBody.velocity.x, 0f, _rigidBody.velocity.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, _rotateSpeed * Time.deltaTime);
        }
        else if (_target != null)
        {
            Quaternion rot = Quaternion.LookRotation(new Vector3(_target.position.x - transform.position.x, 0f, _target.position.z - transform.position.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, _rotateSpeed * Time.deltaTime);
        }
    }

    protected void SwitchState(State newState)
    {
        _state = newState;
        isVictory = isAttack = isDead = isCharge = false;

        switch(_state)
        {
            case State.Attack:
                isAttack = true;
                break;
            case State.Charge:
                isCharge = true;
                break;
            case State.Victory:
                isVictory = true;
                break;
            case State.Fight:
                _rigidBody.constraints = _rigidBody.constraints | RigidbodyConstraints.FreezePositionY;
                break;
            case State.Dead:
                _rigidBody.velocity = Vector3.zero;
                isDead = true;
                deadAnimIndex = Random.Range(0, 2);
                break;
        }
    }

    public void SwitchType(SoldierType type)
    {
        _type = type;
        gameObject.layer = LayerMask.NameToLayer(_type.ToString());

        switch (type)
        {
            case SoldierType.Neutral:
                _mesh.material = _neutralMat;
                break;
            case SoldierType.Friendly:
                _mesh.material = _friendlyMat;

                if (_friendlyScript != null)
                {
                    _friendlyScript.enabled = true;
                    Destroy(this);
                }
                break;
            case SoldierType.Enemy:
                break;
        }
    }

    protected virtual Transform FindClosestTarget()
    {
        LayerMask layer = 0;

        if (_type == SoldierType.Enemy)
            layer = _friendlyLayer;
        else if (_type == SoldierType.Friendly)
            layer = _enemyLayer;

        Collider[] colliders = Physics.OverlapSphere(transform.position, _findRange, layer);
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

    protected void MoveToTarget()
    {
        Vector3 direction = (_target.position - transform.position).normalized;
        _rigidBody.velocity = _moveSpeed * direction;
    }

    private void Attack()
    {
        isAttack = true;
        attackAnimIndex = Random.Range(0, _fightAnimCount);
        _rigidBody.mass = 1000f;
    }

    public void ChargeToTarget(Transform target)
    {
        _chargeTarget = target;
        GetComponent<Collider>().isTrigger = true;
        _rigidBody.useGravity = false;
        SwitchState(State.Charge);
    }

    public virtual bool GetDamage(int damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            Dead(true);
            return true;
        }

        return false;
    }

    public virtual void GiveDamage()
    {
        if (_target == null) return;

        bool targetIsDied = false;

        if (_target.TryGetComponent(out Soldier sold))
            targetIsDied = sold.GetDamage(_damage);
        else
            targetIsDied = false;

        if (targetIsDied && State == State.Fight)
            _target = null;
    }

    public void EndOfAttack()
    {
        _isAttackDelay = false;
        isAttack = false;
        _rigidBody.mass = 1f;
    }

    protected void OnDestroy()
    {

    }

    protected void FightStart()
    {
        if (gameObject.tag == "Defender" || gameObject.tag == "Attacking")
            SwitchState(State.Fight);
        else
            Destroy(gameObject);
    }

    public void NewStepSoundSource()
    {
        isStepSoundSource = true;
    }

    public virtual void Dead(bool destroy)
    {
        if (gameObject.tag == "Defender")
            FightStage.DefenderDied();

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

        gameObject.tag = "Dead";
    }
}
