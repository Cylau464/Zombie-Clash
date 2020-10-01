using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidBody = null;
    [SerializeField] private float _sideSpeed = 25f;
    [SerializeField] private float _moveSpeed = 15f;

    private void FixedUpdate()
    {
        _rigidBody.velocity = Vector3.Scale(InputController.moveDirection, new Vector3(_sideSpeed, 0f, _moveSpeed) * InputController.acceleration);
    }
}
