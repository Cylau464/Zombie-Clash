using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public static Vector3 moveDirection;
    public static InputController current;
    public static bool leftSideBlock, rightSideBlock;
    public static float acceleration;

    [SerializeField] private float _accelerationMultiplier = 2f;

    private Vector3 _lastPosition; // Mouse control

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
            acceleration = Mathf.Clamp(acceleration + Time.deltaTime * _accelerationMultiplier, 0f, 1f);
            Touch t = Input.GetTouch(0);

            if (t.phase != TouchPhase.Canceled && t.phase != TouchPhase.Ended)
            {
                float delta = Mathf.Clamp(t.deltaPosition.x, -5f, 5f);

                if ((delta > 0f && rightSideBlock) || (delta < 0f && leftSideBlock))
                    delta = 0f;

                Vector3 horizontalDirection = new Vector3(delta / 5f, 0f, 0f);
                return (Vector3.forward + horizontalDirection).normalized;
            }
        }
        else if(Input.GetMouseButton(0))
        {
            acceleration = Mathf.Clamp(acceleration + Time.deltaTime * _accelerationMultiplier, 0f, 1f);
            _lastPosition = _lastPosition == Vector3.zero ? Input.mousePosition : _lastPosition;
            Vector3 deltaPosition = Input.mousePosition - _lastPosition;
            _lastPosition = Input.mousePosition;
            float delta = Mathf.Clamp(deltaPosition.x, -5f, 5f);

            if ((delta > 0f && rightSideBlock) || (delta < 0f && leftSideBlock))
                delta = 0f;

            Vector3 horizontalDirection = new Vector3(delta / 5f, 0f, 0f);
            return (Vector3.forward + horizontalDirection).normalized;
        }

        _lastPosition = Vector3.zero;
        acceleration = 0f;
        return Vector3.zero;
    }
}
