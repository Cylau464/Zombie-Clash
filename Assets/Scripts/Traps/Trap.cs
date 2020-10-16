using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] protected LayerMask _friendlyLayer = 0;
    [SerializeField] protected LayerMask _inactiveTrapLayer = 0;

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
        sold.Dead(true);
    }
}
