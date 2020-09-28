using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTrap : Trap
{
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private Vector3 _moveDirection = new Vector3(0f, 1f, 0f);
    [SerializeField] private float _moveDistance = 10f;
    [SerializeField] private float _returnDelay = 1f;

    private Vector3 _targetPos;
    private Vector3 _startPos;

    private void Start()
    {
        _startPos = transform.position;
        _targetPos = _startPos + _moveDirection * _moveDistance;

        StartCoroutine(Move(_targetPos));
    }

    private IEnumerator Move(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > float.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * _moveSpeed);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(_returnDelay);
        
        if(target == _targetPos)
            StartCoroutine(Move(_startPos));
        else
            StartCoroutine(Move(_targetPos));
    }
}
