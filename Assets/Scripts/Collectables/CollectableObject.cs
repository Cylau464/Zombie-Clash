using System.Collections.Generic;
using UnityEngine;

public class CollectableObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Friendly")
        {
            Collect();
        }
    }

    protected virtual void Collect()
    {
        Destroy(gameObject);
    }
}
