using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//受伤数据
public struct HurtData
{
    public Entity hurtEntity; //受伤者
    public bool isCrit; //是否暴击
    public E_ElementType elementType; //元素类型
    public float elementDuration; //元素的持续时间
}
