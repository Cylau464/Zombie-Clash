using UnityEngine;
using System.Collections;

public class CastleSiege : MonoBehaviour
{
    [SerializeField] private Transform _moveTarget = null;
    [SerializeField] private GameObject _particle = null;
    public static Transform moveTarget;

    private void Start()
    {
        moveTarget = _moveTarget;
        FightStage.fightEnd.AddListener(SiegeStart);
    }

    private void SiegeStart()
    {
        AudioManager.PlaySiegeSound();
        Instantiate(_particle, transform.position, Quaternion.identity);
    }
}