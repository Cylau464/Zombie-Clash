using System.Collections.Generic;
using UnityEngine;
using Enums;

public class Soldier : MonoBehaviour
{
    protected State _state = State.Idle;
    public State State
    {
        get { return _state; }
        protected set { _state = value; }
    }

    [SerializeField] protected SoldierType _type = SoldierType.Neutral;

    [Header("Movement Properties")]
    [SerializeField] protected float _moveSpeed = 15f;
    [SerializeField] protected float _sideSpeed = 25f;
    [Space]
    [SerializeField] protected float _chargeSpeed = 30f;

    [Header("Material Colors")]
    [SerializeField] protected Color _neutralColor = new Color32(100, 100, 100, 255);
    [SerializeField] protected Color _friendlyColor = new Color32(50, 50, 200, 255);
    [SerializeField] protected Color _enemyColor = new Color32(200, 30, 30, 255);

    [Header("References")]
    private Material _material = null;
    [SerializeField] protected Rigidbody _rigidBody = null;
    [SerializeField] protected LayerMask _friendlyLayer = 0;
    [SerializeField] protected FriendlySoldier _friendlyScript = null;

    private void Awake()
    {
        _rigidBody = _rigidBody == null ? GetComponent<Rigidbody>() : _rigidBody;
        _material = GetComponent<MeshRenderer>().material;
        SwitchType(_type);
    }

    protected void SwitchState(State newState)
    {
        _state = newState;
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

    public void ChargeToTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        _rigidBody.velocity = _chargeSpeed * direction;
        SwitchState(State.Charge);
    }
}
