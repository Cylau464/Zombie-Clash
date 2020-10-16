using System.Collections.Generic;
using UnityEngine;

public class Wall : Trap
{
    protected override void Trapped(GameObject target)
    {
        base.Trapped(target);
        gameObject.layer = (int)Mathf.Log(_inactiveTrapLayer.value, 2);
        //_friendlyLayer = 0;
    }
}