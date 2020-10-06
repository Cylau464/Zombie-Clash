using System.Collections.Generic;
using UnityEngine;
using Enums;
using UnityEngine.Events;
using System.Collections;

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
    [SerializeField] protected int _maxHealth = 1;
    private int _health;
    [SerializeField] protected int _damage = 1;
    [SerializeField] private float _attackRange = 1f;

    [HideInInspector] public int attackAnimIndex = 0;
    [HideInInspector] public int deadAnimIndex = 0;

    [Header("Charge Properties")]
    protected Transform _chargeTarget;

    [Header("Movement Properties")]
    [SerializeField] protected float _moveSpeed = 15f;
    [SerializeField] protected float _sideSpeed = 25f;
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
    protected Transform _target;

    [Header("Camera Properties")]
    protected bool _isCameraTarget;

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

    protected UnityAction _fightStart;

    private void OnEnable()
    {
        _health = _maxHealth;
        _rigidBody = _rigidBody == null ? GetComponent<Rigidbody>() : _rigidBody;
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
            }
            else if (isAttack == false)
            {
                if (Vector3.Distance(transform.position, _target.position) > _attackRange)
                    MoveToTarget();
                else
                    Attack();
            }
            else
                _rigidBody.velocity = Vector3.zero;
        }
    }

    protected void FixedUpdate()
    {
        if (_rigidBody.velocity.magnitude > .1f)
        {
            Quaternion rot = Quaternion.LookRotation(new Vector3(_rigidBody.velocity.x, 0f, _rigidBody.velocity.z));
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
            case State.Dead:
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
        Debug.Log("MOVE TO TARGET");
    }

    private void Attack()
    {
        isAttack = true;
        attackAnimIndex = Random.Range(0, 2);
    }

    public void ChargeToTarget(Transform target)
    {
        _chargeTarget = target;
        //Vector3 direction = (_chargeTarget.position - transform.position).normalized;
        //_rigidBody.velocity = _chargeSpeed * direction;
        //GetComponent<Collider>().isTrigger = true;
        SwitchState(State.Charge);
    }

    public bool GetDamage(int damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            Destroy(gameObject);
            return true;
        }

        return false;
    }

    public void GiveDamage()
    {
        if (_target == null) return;

        bool targetIsDied = _target.GetComponent<Soldier>().GetDamage(_damage);
        
        if (targetIsDied)
            _target = null;
    }

    public void EndOfAttack()
    {
        isAttack = false;
    }

    protected void OnDestroy()
    { 
        // Spawn blood or something other particle
    }

    protected void FightStart()
    {
        SwitchState(State.Fight);
    }

    public void NewCameraTarget()
    {
        _isCameraTarget = true;
    }

    public virtual IEnumerator DestroySelf()
    {
        Destroy(gameObject);
        yield break;
    }
}
