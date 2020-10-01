using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _roadPrefab = null;
    [SerializeField] private GameObject[] _castlePrefabs = null;
    [SerializeField] private float _minRoadLength = 30f;
    [SerializeField] private float _maxRoadLength = 80f;


    private void CreateRoad()
    {
        MeshRenderer mesh = _roadPrefab.GetComponent<MeshRenderer>();
        Vector3 defMeshSize = mesh.bounds.size;
        Material material = mesh.material;
        float length = Random.Range(_minRoadLength, _maxRoadLength);

        GameObject road = Instantiate(_roadPrefab, Vector3.zero, Quaternion.identity);
        road.transform.localScale = Vector3.forward * length;
        material.mainTextureScale = new Vector2(material.mainTextureScale.x, length / defMeshSize.z);

        GameObject castle = _castlePrefabs[Random.Range(0, _castlePrefabs.Length)];
        Vector3 castleSpawnPos = Vector3.forward * length;
        castleSpawnPos.z += castle.GetComponent<MeshRenderer>().bounds.size.z / 2f;
        Instantiate(castle, castleSpawnPos, Quaternion.identity);
    }
}
