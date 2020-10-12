using System.Collections.Generic;
using UnityEngine;
using Enums;

public class NeutralSoldier : Soldier
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == Mathf.Log(_friendlyLayer.value, 2))
        {
            if (transform.parent != null && transform.parent.TryGetComponent(out SoldierGroup group))
                group.RecruitEveryone();
            else
                SwitchType(SoldierType.Friendly);

            AudioManager.PlayZombiePickupSound();
        }
    }
}