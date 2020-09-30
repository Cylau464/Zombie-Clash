using System.Collections.Generic;
using UnityEngine;
using Enums;
using UnityEngine.Events;

public class Soldier : MonoBehaviour
{
    protected State _state = State.Idle;
    public State State
    {
        get { return _state; }
        protected set { _state = value; }
    }

    [SerializeField] protected SoldierType _type = SoldierType.Neutral;

    [Header("Fight Attributes")]
    [SerializeField] private int _maxHealth = 1;
    private int _health;
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _attackRange = 1f;

    [Header("Movement Properties")]
    [SerializeField] protected float _moveSpeed = 15f;
    [SerializeField] protected float _sideSpeed = 25f;
    [Space]
    [SerializeField] protected float _chargeSpeed = 30f;

    [Header("Material Colors")]
    [SerializeField] protected Color _neutralColor = new Color32(100, 100, 100, 255);
    [SerializeField] protected Color _friendlyColor = new Color32(50, 50, 200, 255);
    [SerializeField] protected Color _enemyColor = new Color32(200, 30, 30, 255);

    [Header("Find Target Properties")]
    [SerializeField] protected float _findRange = 15f;
    protected Transform _target;

    [Header("References")]
    private Material _material = null;
    [SerializeField] protected Rigidbody _rigidBody = null;
    [SerializeField] protected LayerMask _friendlyLayer = 0;
    [SerializeField] private LayerMask _enemyLayer = 0;
    [SerializeField] protected FriendlySoldier _friendlyScript = null;

    [Header("Status Flags")]
    public bool isCharge;
    public bool isFight;
    public bool isAttack;

    protected UnityAction _fightStart;

    private void Awake()
    {
        _health = _maxHealth;
        _rigidBody = _rigidBody == null ? GetComponent<Rigidbody>() : _rigidBody;
        _material = GetComponent<MeshRenderer>().material;
        SwitchType(_type);
        _fightStart = FightStart;
    }

    protected void Update()
    {
        if(_state == State.Fight)
        {
            if (_target == null)
            {
                _target = FindClosestTarget();
                _rigidBody.velocity = transform.forward * _moveSpeed;
                
                if (_target == null) return;
            }

            if (isAttack == false)
            {
                if (Vector3.Distance(transform.position, _target.position) > _attackRange)
                    MoveToTarget();
                else
                    Attack();
            }
        }
    }

    protected void SwitchState(State newState)
    {
        _state = newState;
        isCharge = isAttack = isFight = false;

        switch(_state)
        {
            case State.Fight:
                isFight = true;
                break;
            case State.Charge:
                isCharge = true;
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
                _material.color = _neutralColor;
                break;
            case SoldierType.Friendly:
                _material.color = _friendlyColor;

                if (_friendlyScript != null)
                {
                    _friendlyScript.enabled = true;
                    Destroy(this);
                }
                break;
            case SoldierType.Enemy:
                _material.color = _enemyColor;
                break;
        }
    }

    protected Transform FindClosestTarget()
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
            distanceToTarget = Vector3.Distance(transform.position, col.transform.position);

            if (distanceToTarget < nearestDistanceToTarget && col.GetComponent<Soldier>().State != State.Charge)
            {
                nearestDistanceToTarget = distanceToTarget;
                target = col.transform;
            }
        }

        return target;
    }

    private void MoveToTarget()
    {
        Vector3 direction = (_target.position - transform.position).normalized;
        _rigidBody.velocity = _moveSpeed * direction;
    }

    private void Attack()
    {
        isAttack = true;
        _rigidBody.velocity = Vector3.zero;
    }

    private void EndOfAttack()
    {
        isAttack = false;
    }

    public void ChargeToTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        _rigidBody.velocity = _chargeSpeed * direction;
        SwitchState(State.Charge);
    }

    public void GetDamage(int damage)
    {
        _health -= damage;

        if (_health <= 0)
            Destroy(gameObject);
    }

    private void GiveDamage()
    {
        _target.GetComponent<Soldier>().GetDamage(_damage);
    }

    protected void OnDestroy()
    {
        // Spawn blood or something other particle
    }

    protected void FightStart()
    {
        SwitchState(State.Fight);
    }
}
