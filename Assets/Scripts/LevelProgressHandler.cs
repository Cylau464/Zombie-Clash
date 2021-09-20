using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProgressHandler : MonoBehaviour
{
    [SerializeField] private float _lengthOffset = 5f;
    private Transform _road;
    private float _startPosZ;
    private float _finishPosZ;
    private float _roadLength;
    private float _progress;
    public static float Progress
    {
        get { return current._progress; }
    }

    private static LevelProgressHandler current;

    private void Awake()
    {
        if (current != null && current != this)
        {
            Destroy(this);
            return;
        }

        current = this;
    }

    void Start()
    {
        _roadLength = RoadSpawner.roadLength - _lengthOffset;
        _startPosZ = transform.position.z - _lengthOffset;
        _finishPosZ = _startPosZ + _roadLength;
    }

    void FixedUpdate()
    {
        if (_progress < 1f)
        {
            _progress = Mathf.Lerp(_startPosZ, _finishPosZ, (transform.position.z - _lengthOffset) / _roadLength) / _roadLength;
            TopBar.UpdateLevelProgress(_progress);
        }
    }
}
