using System.Collections.Generic;
using UnityEngine;

public class Spin : Trap
{
    [SerializeField] private float _rotateSpeed = 360f;
    [SerializeField] private Vector3Int _rotateAxis = new Vector3Int(0, 1, 0);

    private void Update()
    {
        transform.Rotate(_rotateAxis, _rotateSpeed * Time.deltaTime, Space.World);
    }
}
