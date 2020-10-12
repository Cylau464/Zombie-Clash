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
    private static int defendersLeft = 0;
    [SerializeField] private float _spawnInterval = .8f;

    [Header("Boss")]
    public static int bossHealthLeft;

    [Space]
    [SerializeField] private LayerMask _friendlyLayer = 0;
    [SerializeField] private GameObject _defenderGroup = null;
    [SerializeField] private CinemachineVirtualCamera _fightCamera = null;

    private static FightStage current;

    public static UnityEvent fightStart;

    private void Awake()
    {
        if (current != null && current != this)
        {
            Destroy(this);
            return;
        }

        current = this;
        fightStart = new UnityEvent();

        if (defendersLeft > 0)
            _defendersCount = defendersLeft;
    }

    private void Start()
    {
        for(int i = 0, j = 1; i < _defendersCount; i++)
        {
            int direction = i % 2f == 0 ? 1 : -1;
            float xPos = _spawnInterval * j * direction;
            Vector3 spawnPos = _defenderGroup.transform.position;
            spawnPos.x += xPos;

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
    }

    public static void DefenderDied()
    {
        current._defendersCount--;
        defendersLeft = current._defendersCount;

        if (current._defendersCount <= 0)
            GameManager.levelCompleted.Invoke();
    }
}
