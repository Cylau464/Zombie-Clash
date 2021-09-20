using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Enums;
using UnityEngine.Events;
using Cinemachine;

public class FightStage : MonoBehaviour
{
    [Header("Soldiers Spawn")]
    [SerializeField] private GameObject _defenderPrefab = null;
    [SerializeField] private int _defendersCount = 10;
    public static int defendersLeft = 0;
    [SerializeField] private float _spawnInterval = .8f;

    [Header("Boss")]
    [SerializeField] private bool _bossLevel = false;
    public static int bossHealthLeft;

    [Space]
    [SerializeField] private LayerMask _friendlyLayer = 0;
    [SerializeField] private GameObject _defenderGroup = null;
    [SerializeField] private CinemachineVirtualCamera _fightCamera = null;
    [SerializeField] private CinemachineVirtualCamera _fightEndCamera = null;

    private static FightStage current;

    public static UnityEvent fightStart;
    public static UnityEvent fightEnd;
    public static bool fightBegan;

    private void Awake()
    {
        if (current != null && current != this)
        {
            Destroy(this);
            return;
        }

        current = this;
        fightStart = new UnityEvent();
        fightEnd = new UnityEvent();
        fightBegan = false;

        if (_bossLevel == false)
        {
            if (defendersLeft > 0)
                _defendersCount = defendersLeft;
            else
                _defendersCount += LevelManager.LevelNumber / 2;
        }
        else
        {
            _defendersCount = 1;
        }
    }

    private void Start()
    {
        float zPos = 0f;

        for (int i = 0, j = 1; i < _defendersCount; i++)
        {
            if (i % 12 == 0)
            {
                zPos += i / 12;
                j = 1;
            }

            int direction = i % 2f == 0 ? 1 : -1;
            float xPos = _defendersCount > 1 ? _spawnInterval * j * direction : 0f;
            Vector3 spawnPos = _defenderGroup.transform.position;
            spawnPos.x += xPos;
            spawnPos.z += zPos;
            spawnPos.y = _defenderPrefab.transform.position.y;

            Instantiate(_defenderPrefab, spawnPos, _defenderPrefab.transform.rotation, _defenderGroup.transform).tag = "Defender";

            if (direction < 0)
                j++;
        }

        _defenderGroup.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Mathf.Log(_friendlyLayer.value, 2) && _defenderGroup.activeSelf == false)
        {
            StartCoroutine(FightStart());
        }
    }

    private IEnumerator FightStart()
    {
        _fightCamera.Priority += 10;
        _defenderGroup.SetActive(true);
        yield return new WaitForEndOfFrame();
        fightStart.Invoke();
        AudioManager.PlayFightSound();
        fightBegan = true;
    }

    public static void DefenderDied()
    {
        current._defendersCount--;
        defendersLeft = current._defendersCount;

        if (current._defendersCount <= 0)
        {
            fightEnd.Invoke();
            current._fightEndCamera.Priority += 50;
            current.Invoke(nameof(Completed), 4f);
        }
    }

    private void Completed()
    {
        GameManager.levelCompleted.Invoke();
    }
}
