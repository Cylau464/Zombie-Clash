using System.Collections.Generic;
using UnityEngine;
using Enums;

public class SoldierGroup : MonoBehaviour
{
    [SerializeField] private List<Soldier> _soldiers = new List<Soldier>();
    [SerializeField] private BoxCollider _myCollider;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Friendly")
        {
            transform.Rotate(Vector3.up, 180f);
            gameObject.tag = "Friendly";

            foreach(Soldier sold in _soldiers)
            {
                sold.SwitchType(SoldierType.Friendly, "Friendly");
            }

            _myCollider.enabled = false;
        }
    }
}
