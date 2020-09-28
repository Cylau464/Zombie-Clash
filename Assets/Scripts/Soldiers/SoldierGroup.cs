using System.Collections.Generic;
using UnityEngine;
using Enums;

public class SoldierGroup : MonoBehaviour
{
    [SerializeField] private List<Soldier> _soldiers = new List<Soldier>();

    public void RecruitEveryone()
    {
        foreach (Soldier sold in _soldiers)
        {
            sold.SwitchType(SoldierType.Friendly);
        }
    }
}
