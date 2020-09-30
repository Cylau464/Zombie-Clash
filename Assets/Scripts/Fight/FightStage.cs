using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Enums;
using UnityEngine.Events;

public class FightStage : MonoBehaviour
{
    [SerializeField] private Transform _defendersSpawnPos = null;
    [SerializeField] private GameObject _defenderPrefabs = null;
    [SerializeField] private int _defendersCount = 1;
    private int _curDefendersCount;
    [SerializeField] private LayerMask _friendlyLayer = 0;

    private List<GameObject> _defenders = new List<GameObject>();
    private Coroutine _coroutine;

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
        for(int i = 0; i < _defendersCount; i++)
        {
            _defenders.Add(Instantiate(_defenderPrefabs, _defendersSpawnPos.position, Quaternion.identity));
            _defenders[i].SetActive(false);
        }

        _curDefendersCount = _defendersCount;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == Mathf.Log(_friendlyLayer.value, 2) && _coroutine == null)
        {
            _coroutine = StartCoroutine(ActivateDefenders());
            fightStart.Invoke();
        }
    }

    private IEnumerator ActivateDefenders()
    {
        foreach (GameObject defender in _defenders)
        {
            defender.SetActive(true);
            yield return new WaitForSeconds(.2f);
        }
    }

    public static void DefenderDied()
    {
        current._curDefendersCount--;

        if (current._curDefendersCount <= 0)
            GameManager.levelCompleted.Invoke();
    }
}
