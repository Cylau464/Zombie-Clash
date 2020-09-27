using System.Collections.Generic;
using UnityEngine;
using Enums;

public class Soldier : MonoBehaviour
{
    private enum State { Idle, Run, Fight }
    private State _state = State.Idle;

    [SerializeField] private SoldierType _type = SoldierType.Neutral;

    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _sideSpeed = 20f;

    [Header("Material Colors")]
    [SerializeField] private Color _neutralColor = new Color(150f, 150f, 150f);
    [SerializeField] private Color _friendlyColor = new Color(0f, 0f, 150f);
    [SerializeField] private Color _enemyColor = new Color(150, 0f, 0f);

    [Header("References")]
    private Material _material = null;
    [SerializeField] private Rigidbody _rigidBody = null;

    [SerializeField] private LayerMask _roadEdgeLayer;

    private void Awake()
    {
        _material = GetComponent<MeshRenderer>().material;
    }

    private void FixedUpdate()
    {
        switch (_state)
        {
            case State.Run:
            case State.Idle:
                switch (_type)
                {
                    case SoldierType.Friendly:
                        _rigidBody.velocity = Vector3.Scale(InputController.moveDirection, new Vector3(_sideSpeed, 0f, _moveSpeed));
                        break;
                }
                break;
        }
    }

    public void SwitchType(SoldierType type, string tag)
    {
        _type = type;
        gameObject.tag = tag;

        switch (type)
        {
            case SoldierType.Neutral:
                _material.color = _neutralColor;
                break;
            case SoldierType.Friendly:
                _material.color = _friendlyColor;
                break;
            case SoldierType.Enemy:
                _material.color = _enemyColor;
                break;
        }
    }

    private void OnDestroy()
    {
        if (_type == SoldierType.Enemy)
            FightStage.DefenderDied();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == Mathf.Log(_roadEdgeLayer.value, 2))
        {
            // Right side
            if(collision.transform.position.x > transform.position.x)
            {
                InputController.rightSideBlock = true;
            }
            // Left side
            else if(collision.transform.position.x < transform.position.x)
            {
                InputController.leftSideBlock = true;
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
            }
            // Left side
            else if (collision.transform.position.x < transform.position.x)
            {
                InputController.leftSideBlock = false;
            }
        }
    }
}
