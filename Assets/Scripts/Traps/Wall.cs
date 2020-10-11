using System.Collections.Generic;
using UnityEngine;

public class Wall : Trap
{
    protected override void Trapped(GameObject target)
    {
        base.Trapped(target);
        _friendlyLayer = 0;
    }
}