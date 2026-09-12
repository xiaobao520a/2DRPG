using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//一条属性加成 例如: 一把剑 = { PhysicalDamage, 3 } + { CritChance, 2 }
//装备/技能/Buff把它们各自的一串Modifier 通过 Entity_Attribute.ApplyModifier 应用
[System.Serializable]
public struct AttributeModifier
{
    public E_AttributeType attributeType;  //加成哪个属性
    public float value;          //加多少

    public AttributeModifier(E_AttributeType attributeType, float value)
    {
        this.attributeType = attributeType;
        this.value = value;
    }

}
