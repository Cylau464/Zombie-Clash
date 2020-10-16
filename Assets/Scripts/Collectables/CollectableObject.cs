using System.Collections.Generic;
using UnityEngine;

public class CollectableObject : MonoBehaviour
{
    [SerializeField] private LayerMask _friendlyLayer = 0;
    [SerializeField] private AudioClip[] _collectClips = null;
    [SerializeField] private GameObject _collectParticle = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Mathf.Log(_friendlyLayer.value, 2))
        {
            Collect();
        }
    }

    protected virtual void Collect()
    {
        AudioManager.PlayClipAtPosition(_collectClips[Random.Range(0, _collectClips.Length)], transform.position);
        Instantiate(_collectParticle, transform.position, _collectParticle.transform.rotation);
        Destroy(gameObject);
    }
}
