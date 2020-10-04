using System.Collections.Generic;
using UnityEngine;
using Enums;
using Cinemachine;

public class SoldierGroup : MonoBehaviour
{
    public List<Soldier> _soldiers = new List<Soldier>();

    public void RecruitEveryone()
    {
        GameObject mainGroup = GameObject.FindGameObjectWithTag("Main Group");
        SoldierGroup soldGroup = mainGroup.GetComponent<SoldierGroup>();

        foreach (Soldier sold in _soldiers)
        {
            sold.SwitchType(SoldierType.Friendly);
            sold.transform.parent = mainGroup.transform;
        }

        Destroy(gameObject);
    }
}
