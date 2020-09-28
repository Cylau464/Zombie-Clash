using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public static Vector3 moveDirection;
    public static InputController current;
    public static bool leftSideBlock, rightSideBlock;
    public static float acceleration;

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
        if (Input.touches.Length > 0)
        {
            acceleration = Mathf.Clamp(acceleration + Time.deltaTime * 2f, 0f, 1f);
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Began)
            {
                _startTouchPos = t.position;
                return Vector3.forward;
            }
            else if (t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary)
            {
                //_curTouchPos = t.position;
                //_curTouchPos = new Vector3(_curTouchPos.x - _startTouchPos.x, 0f, 0f);
                float delta = Mathf.Clamp(t.deltaPosition.x, -5f, 5f);

                if ((delta > 0f && rightSideBlock) || (delta < 0f && leftSideBlock))
                    delta = 0f;

                _curTouchPos = new Vector3(delta / 5f, 0f, 0f);
                _startTouchPos = t.position;
                //Debug.Log(_curTouchPos + " DIRECTION " + _curTouchPos * 5f + " DIR " + _curTouchPos.normalized + " NORMALIZE");
                return (Vector3.forward + _curTouchPos).normalized;
            }
        }

        acceleration = 0f;
        return Vector3.zero;
    }
}
