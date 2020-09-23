using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public Vector3 moveDirection;

    private Vector3 _startTouchPos;
    private Vector3 _curTouchPos;

    private void Update()
    {
        moveDirection = GetMoveDirection();
    }

    private Vector3 GetMoveDirection()
    {
        if(Input.touches.Length > 0)
        {
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Began)
            {
                _startTouchPos = t.position;
                return Vector3.forward;
            }
            else if (t.phase == TouchPhase.Moved)
            {
                _curTouchPos = t.position;
                _curTouchPos = new Vector3(_curTouchPos.x - _startTouchPos.x, 0f, 0f);
                _startTouchPos = t.position;
                return (Vector3.forward + _curTouchPos).normalized;
            }
        }

        return Vector3.zero;
    }
}
