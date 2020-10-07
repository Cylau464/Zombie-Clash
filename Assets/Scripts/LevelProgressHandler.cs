﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProgressHandler : MonoBehaviour
{
    private Transform _road;
    private float _startPosZ;
    private float _finishPosZ;
    private float _roadLength;

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
        _roadLength = RoadSpawner.roadLength;
        _startPosZ = transform.position.z;
        _finishPosZ = _startPosZ + _roadLength;
    }

    void FixedUpdate()
    {
        float progress = Mathf.Lerp(_startPosZ, _finishPosZ, transform.position.z / _roadLength) / _roadLength;
        TopBar.UpdateLevelProgress(progress);
    }
}