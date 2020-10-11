using UnityEngine;
using System.Collections;
using Cinemachine;
using UnityEngine.Events;
using UnityEngine.Assertions.Must;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _startGameCamera = null;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera = null;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera2 = null;
    [SerializeField] private Camera _overlayCamera = null;
    private Transform _cameraTarget;
    private Transform _cameraTarget2;

    public static CameraSwitch current;
    public static UnityEvent resetTargetEvent;
    private static UnityAction _resetTarget;

    private void Awake()
    {
        if (current != null && current != this)
        {
            Destroy(gameObject);
            return;
        }

        current = this;
        _cameraTarget = _virtualCamera.Follow;
        _cameraTarget2 = _virtualCamera2.Follow;
        
        resetTargetEvent = new UnityEvent();
        _resetTarget = ResetTarget;
        resetTargetEvent.AddListener(_resetTarget);
        resetTargetEvent.Invoke();

        GameObject.FindGameObjectWithTag("UI Main").GetComponent<Canvas>().worldCamera = _overlayCamera;
    }

    private void Start()
    {
        GameManager.gameStart.AddListener(TurnOffStartCamera);
    }

    //private void OnDisable()
    //{
    //    //resetTargetEvent.RemoveAllListeners();
    //}

    public void ResetTarget()
    {
        Transform group = GameObject.FindGameObjectWithTag("Main Group")?.transform;
        float nearestXPos = float.MaxValue;
        Transform newTarget = null;

        if (group == null) return;

        foreach(Transform t in group)
        {
            if (t.tag != "Dead" && Mathf.Abs(t.localPosition.x) < nearestXPos)
                newTarget = t;
        }

        if (newTarget == null) return;

        newTarget.GetComponent<Soldier>().NewCameraTarget();

        if (_virtualCamera.Priority > _virtualCamera2.Priority)
        {
            Vector3 targetPos = _cameraTarget2.position;
            targetPos.x = newTarget.position.x;
            _cameraTarget2.position = targetPos;
            _virtualCamera2.Priority++;
            _virtualCamera.Priority--;
        }
        else
        {
            Vector3 targetPos = _cameraTarget.position;
            targetPos.x = newTarget.position.x;
            _cameraTarget.position = targetPos;
            _virtualCamera.Priority++;
            _virtualCamera2.Priority--;
        }
    }

    private void TurnOffStartCamera()
    {
        _startGameCamera.Priority = 0;
    }
}