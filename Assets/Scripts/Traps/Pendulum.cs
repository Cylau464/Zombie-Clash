using UnityEngine;
using System.Collections;

public class Pendulum : Trap
{
    [SerializeField] private Transform _pivot = null;
    [SerializeField] private float _maxAngle = 60f;
    [SerializeField] private float _speed = 2f;

    private void FixedUpdate()
    {
        float angle = _maxAngle * Mathf.Sin(Time.time * _speed);
        _pivot.localRotation = Quaternion.Euler(0, 0, angle);
    }
}