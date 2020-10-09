using UnityEngine;
using System.Collections;

public class Pendulum : Trap
{
    [SerializeField] private Transform _pivot = null;
    [SerializeField] private float _maxAngle = 60f;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private int _direction = 1;
    private float _targetRotZ;

    private void Awake()
    {
        _pivot.transform.Rotate(Vector3.forward, _maxAngle * -_direction);
        _targetRotZ = _pivot.transform.rotation.eulerAngles.z;

        if (_targetRotZ > 180) _targetRotZ -= 360;

        _targetRotZ += _maxAngle * 2f * _direction;
    }

    private void FixedUpdate()
    {
        float angle = _maxAngle * Mathf.Sin(Time.time * _speed);
        _pivot.localRotation = Quaternion.Euler(0, 0, angle);

        //float angle = _pivot.transform.rotation.eulerAngles.z;
        
        //if (angle > 180) angle -= 360;

        //Debug.Log(angle + " A " + _targetRotZ);

        //if (angle < _targetRotZ)
        //    _pivot.transform.Rotate(Vector3.forward, /*Mathf.Sin(Time.time)*/Time.fixedDeltaTime * _speed * _direction);
        //else
        //{
        //    _direction *= -1;
        //    Debug.Log("BEFORE " + _targetRotZ);
        //    _targetRotZ = angle;

        //    if (_targetRotZ > 180) _targetRotZ -= 360;

        //    _targetRotZ += _maxAngle * 2f * _direction;
        //    Debug.Log("AFter " + _targetRotZ);
        //}
    }
}