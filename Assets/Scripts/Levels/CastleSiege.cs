using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class CastleSiege : MonoBehaviour
{
    [SerializeField] private Transform _moveTarget = null;
    [SerializeField] private GameObject[] _particles = null;
    public static Transform moveTarget;

    public static UnityEvent siegeStart;

    private void Awake()
    {
        siegeStart = new UnityEvent();
    }

    private void Start()
    {
        moveTarget = _moveTarget;
        siegeStart.AddListener(SiegeStart);
    }

    private void SiegeStart()
    {
        AudioManager.PlaySiegeSound();

        foreach (GameObject particle in _particles)
        {
            Vector3 spawnPos = transform.position;
            spawnPos.y = particle.transform.position.y;

            Instantiate(particle, spawnPos, particle.transform.rotation);
        }
    }
}