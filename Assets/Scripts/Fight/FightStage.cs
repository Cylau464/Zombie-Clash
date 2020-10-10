using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Enums;
using UnityEngine.Events;
using Cinemachine;

public class FightStage : MonoBehaviour
{
    [SerializeField] private LayerMask _friendlyLayer = 0;
    [SerializeField] private GameObject _defenderGroup = null;
    [SerializeField] private CinemachineVirtualCamera _fightCamera = null;
    private int _defendersCount;

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
    }

    private void Start()
    {
        foreach(Transform child in _defenderGroup.transform)
        {
            child.tag = "Defender";
        }

        _defendersCount = _defenderGroup.transform.childCount;
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
    }

    public static void DefenderDied()
    {
        current._defendersCount--;

        if (current._defendersCount <= 0)
            GameManager.levelCompleted.Invoke();
    }
}
