using System.Collections.Generic;
using UnityEngine;

public class CollectableObject : MonoBehaviour
{
    [SerializeField] private LayerMask _friendlyLayer = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Mathf.Log(_friendlyLayer.value, 2))
        {
            Collect();
        }
    }

    protected virtual void Collect()
    {
        Destroy(gameObject);
    }
}
