using System.Collections.Generic;
using UnityEngine;
using Enums;

public class FriendlySoldier : Soldier
{
    [SerializeField] private LayerMask _roadEdgeLayer;

    private void FixedUpdate()
    {
        switch (_state)
        {
            case State.Run:
            case State.Idle:
                _rigidBody.velocity = Vector3.Scale(InputController.moveDirection, new Vector3(_sideSpeed, 0f, _moveSpeed) * InputController.acceleration);
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
            }
            // Left side
            else if (collision.transform.position.x < transform.position.x)
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