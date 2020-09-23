using System.Collections.Generic;
using UnityEngine;
using Enums;

public class Soldier : MonoBehaviour
{
    private enum State { Idle, Run, Fight }
    private State _state = State.Idle;

    [SerializeField] private SoldierType _type = SoldierType.Neutral;

    [SerializeField] private float _moveSpeed = 5f;

    [Header("Material Colors")]
    [SerializeField] private Color _neutralColor = new Color(150f, 150f, 150f);
    [SerializeField] private Color _friendlyColor = new Color(0f, 0f, 150f);
    [SerializeField] private Color _enemyColor = new Color(150, 0f, 0f);

    [Header("References")]
    [SerializeField] private Material _material = null;
    [SerializeField] private Rigidbody _rigidBody = null;
    private InputController _input;

    [SerializeField] private LayerMask _obstacleLayer;

    private void Awake()
    {
        //_material = GetComponent<Material>();
    }

    private void FixedUpdate()
    {
        switch (_state)
        {
            case State.Run:
                switch (_type)
                {
                    case SoldierType.Friendly:
                        _rigidBody.velocity = _input.moveDirection * _moveSpeed;
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
                _input = _input == null ? gameObject.AddComponent<InputController>() : _input;
                break;
            case SoldierType.Enemy:
                _material.color = _enemyColor;
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == Mathf.Log(_obstacleLayer.value, 2))
        {
            Destroy(gameObject);
        }
    }
}
