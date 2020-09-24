using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Friendly")
        {
            Trapped(collision.gameObject);
        }
    }

    protected virtual void Trapped(GameObject target)
    {
        Destroy(target);
    }
}
