using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _roadPrefab = null;
    [SerializeField] private GameObject _fightArea = null;
    public static float roadLength;

    void Awake()
    {
        Transform road = GameObject.FindGameObjectWithTag("Road").transform;
        roadLength = road.localScale.z;
        int roadPrefabsCount = Mathf.FloorToInt(roadLength / _roadPrefab.transform.localScale.z);
        Vector3 spawnPos = Vector3.zero;
        spawnPos.z += _roadPrefab.transform.localScale.z / 2f;

        for (int i = 0; i < roadPrefabsCount; i++)
        {
            Instantiate(_roadPrefab, spawnPos, Quaternion.identity, transform);
            spawnPos.z += _roadPrefab.transform.localScale.z;
        }

        Destroy(road.gameObject);
    }
}
