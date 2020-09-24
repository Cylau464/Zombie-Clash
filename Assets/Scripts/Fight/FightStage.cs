using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class FightStage : MonoBehaviour
{
    [SerializeField] private Vector3 _defendersSpawn = Vector3.zero;
    [SerializeField] private GameObject _defenderPrefabs = null;
    [SerializeField] private int _defendersCount = 1;

    private List<GameObject> _defenders = new List<GameObject>();
    private int _curDefendersCount;

    private static FightStage current;

    private void Awake()
    {
        if (current != null && current != this)
        {
            Destroy(this);
            return;
        }

        current = this;
    }

    private void Start()
    {
        for(int i = 0; i < _defendersCount; i++)
        {
            _defenders.Add(Instantiate(_defenderPrefabs, _defendersSpawn, Quaternion.identity));
            _defenders[i].SetActive(false);
        }

        _curDefendersCount = _defendersCount;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Friendly")
        {
            StartCoroutine(ActivateDefenders());
        }
    }

    private IEnumerator ActivateDefenders()
    {
        foreach(GameObject defender in _defenders)
        {
            defender.SetActive(true);
            yield return new WaitForSeconds(.5f);
        }
    }

    public static void DefenderDied()
    {
        current._curDefendersCount--;

        if (current._curDefendersCount <= 0)
            GameManager.levelCompleted.Invoke();
    }
}
