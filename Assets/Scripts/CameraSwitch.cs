using UnityEngine;
using System.Collections;
using Cinemachine;
using UnityEngine.Events;
using UnityEngine.Assertions.Must;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] private CinemachineBrain _cameraBrain = null;
    [SerializeField] private CinemachineVirtualCamera _startGameCamera = null;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera = null;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera2 = null;
    [SerializeField] private Camera _overlayCamera = null;
    private Transform _cameraTarget;
    private Transform _cameraTarget2;
    private float _blendingDelay;

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

    public void ResetTarget()
    {
        Transform group = GameObject.FindGameObjectWithTag("Main Group")?.transform;
        Transform newTarget = null;
        Transform stepSoundSource = null;
        float maxLeftPos = float.MaxValue;
        float maxRightPos = float.MinValue;

        if (group == null) return;

        foreach(Transform sold in group)
        {
            if(sold.tag != "Dead")
            {
                if (sold.localPosition.x < maxLeftPos)
                    maxLeftPos = sold.localPosition.x;

                if (sold.localPosition.x > maxRightPos)
                    maxRightPos = sold.localPosition.x;

                newTarget = sold;

                if (sold.GetComponent<Soldier>().isStepSoundSource == true)
                    stepSoundSource = sold;
            }
        }

        newTarget = stepSoundSource ?? newTarget;
        float newCameraPosX = Mathf.Lerp(maxLeftPos, maxRightPos, .5f);
        if (newTarget == null) return;

        newTarget.GetComponent<Soldier>().NewStepSoundSource();

        if (_cameraBrain.IsBlending == true || _blendingDelay > Time.time) return;
        
        if (_virtualCamera.Priority > _virtualCamera2.Priority)
        {
            Vector3 targetPos = _cameraTarget2.position;
            targetPos.x = newCameraPosX;
            targetPos.z = Mathf.Lerp(newTarget.position.z, _cameraTarget2.position.z, .5f);
            _cameraTarget2.position = targetPos;
            _virtualCamera2.Priority++;
            _virtualCamera.Priority--;
        }
        else
        {
            Vector3 targetPos = _cameraTarget.position;
            targetPos.x = newCameraPosX;
            targetPos.z = Mathf.Lerp(newTarget.position.z, _cameraTarget.position.z, .5f);
            _cameraTarget.position = targetPos;
            _virtualCamera.Priority++;
            _virtualCamera2.Priority--;
        }

        _blendingDelay = Time.time + .1f;
    }

    private void TurnOffStartCamera()
    {
        _startGameCamera.Priority = 0;
    }
}