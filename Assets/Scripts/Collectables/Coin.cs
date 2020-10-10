using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
public class Coin : CollectableObject
{
    [Header("Animation Properties")]
    [SerializeField] private float _rotationSpeed = 180f;
    [SerializeField] private float _verticalSpeed = 2f;
    [SerializeField] private float _verticalAmplitude = .5f;
    private float _startPosY;
    [Space]
    [SerializeField] private int _score = 1;

    private void Start()
    {
        _startPosY = transform.position.y;
    }

    private void Update()
    {
        //Rotation
        float rot = _rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, rot, Space.World);
        //Amplitude moving
        Vector3 pos = transform.position;
        pos.y = _startPosY + (_verticalAmplitude * Mathf.Sin(Time.time * _verticalSpeed));
        transform.position = pos;
    }

    protected override void Collect()
    {
        GameManager.CollectCoins(_score * UpgradeStats.coinsMultiplier);
        Destroy(gameObject);
    }
}
