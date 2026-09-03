using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//可以被反击的接口 可以被反击的enemy 物体什么的都应该继承它
public interface ICountered
{
    //是否能够被反击
    public bool CanBeCountered { get; set; }

    //被反击
    public void Countered(AttackHitData hitData);
}
