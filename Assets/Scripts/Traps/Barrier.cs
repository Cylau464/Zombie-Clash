using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : Trap
{
    [SerializeField] private Transform _pivotTransform = null;
    [SerializeField] private float _rotationAngle = 180f;
    [SerializeField] private float _rotationSpeed = 1f;
    [SerializeField] private float _waitTime = 1f;
    private int _rotateDirection = 1;
    private Quaternion _targetRotation;

    private void Start()
    {
        StartCoroutine(Rotate());
    }

    private IEnumerator Rotate()
    {
        yield return new WaitForSeconds(_waitTime);

        Vector3 targetEuler = _targetRotation.eulerAngles;
        targetEuler.x += _rotationAngle * _rotateDirection;
        _targetRotation = Quaternion.Euler(targetEuler);
        float duration = Time.time + _rotationSpeed;

        while (duration > Time.time)
        {
            _pivotTransform.Rotate(Vector3.right * _rotateDirection, _rotationAngle * Time.deltaTime * _rotationSpeed, Space.Self);
            yield return new WaitForEndOfFrame();
        }

        _pivotTransform.localRotation = _targetRotation;
        _rotateDirection *= -1;
        StartCoroutine(Rotate());
    }
}