using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTrap : Trap
{
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private Vector3 _moveDirection = new Vector3(0f, 1f, 0f);
    [SerializeField] private Vector3 _moveSecondDirection = Vector3.zero;
    [SerializeField] private float _moveDistance = 10f;
    [SerializeField] private float _moveSecondDistance = 10f;
    [SerializeField] private float _returnDelay = 1f;

    private Vector3 _targetPos;
    private Vector3 _secondTargetPos;
    private Vector3 _startPos;

    private void Start()
    {
        _startPos = transform.position;
        _targetPos = _startPos + _moveDirection * _moveDistance;
        _secondTargetPos = _targetPos + _moveSecondDirection * _moveSecondDistance;

        StartCoroutine(Move(_targetPos, _secondTargetPos));
    }

    private IEnumerator Move(Vector3 target, Vector3 secondTarget = default)
    {
        while (Vector3.Distance(transform.position, target) > float.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * _moveSpeed);
            yield return new WaitForEndOfFrame();
        }

        if(secondTarget != Vector3.zero)
        {
            while (Vector3.Distance(transform.position, secondTarget) > float.Epsilon)
            {
                transform.position = Vector3.MoveTowards(transform.position, secondTarget, Time.deltaTime * _moveSpeed);
                yield return new WaitForEndOfFrame();
            }
        }

        yield return new WaitForSeconds(_returnDelay);

        if (secondTarget != Vector3.zero)
        {
            if (secondTarget == _startPos)
                StartCoroutine(Move(_targetPos, _secondTargetPos));
            else
                StartCoroutine(Move(_targetPos, _startPos));
        }
        else
        {
            if (target == _targetPos)
                StartCoroutine(Move(_startPos));
            else
                StartCoroutine(Move(_targetPos));
        }
    }
}
