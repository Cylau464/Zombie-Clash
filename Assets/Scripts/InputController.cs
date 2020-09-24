using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public static Vector3 moveDirection;
    public static InputController current;

    private Vector3 _startTouchPos;
    private Vector3 _curTouchPos;

    private void Awake()
    {
        if (current != null && current != this)
        {
            Destroy(this);
            return;
        }

        current = this;

        DontDestroyOnLoad(gameObject);
    }

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
            else if(t.phase == TouchPhase.Stationary)
            {
                return Vector3.forward;
            }
        }

        return Vector3.zero;
    }
}
