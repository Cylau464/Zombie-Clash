using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private LayerMask _friendlyLayer = 0;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == Mathf.Log(_friendlyLayer.value, 2))
        {
            Trapped(collision.gameObject);
        }
    }

    protected virtual void Trapped(GameObject target)
    {
        Soldier sold = target.GetComponent<Soldier>();
        sold.StartCoroutine(sold.DestroySelf());
    }
}
