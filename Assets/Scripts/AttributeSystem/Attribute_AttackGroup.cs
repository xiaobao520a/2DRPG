using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//攻击相关属性
[Serializable]
public class Attribute_AttackGroup
{
    public Attribute damage = new Attribute(10); //物理攻击
    public Attribute critPower = new Attribute(150); //暴击伤害(%)
    public Attribute critChance = new Attribute(5); //暴击率(%)
    public Attribute armorPenetration = new Attribute(0); //护甲穿透率(%)

    //元素攻击
    public Attribute fireDamage = new Attribute(0); //火
    public Attribute iceDamage = new Attribute(0); //冰
    public Attribute lightningDamage = new Attribute(0); //闪电

    public Attribute_AttackGroup()
    {
    }

    //深拷贝构造函数
    public Attribute_AttackGroup(Attribute_AttackGroup source)
    {
        damage = new Attribute(source != null && source.damage != null ? source.damage.Value : 0f);
        critPower = new Attribute(source != null && source.critPower != null ? source.critPower.Value : 0f);
        critChance = new Attribute(source != null && source.critChance != null ? source.critChance.Value : 0f);
        armorPenetration = new Attribute(source != null && source.armorPenetration != null ? source.armorPenetration.Value : 0f);
        fireDamage = new Attribute(source != null && source.fireDamage != null ? source.fireDamage.Value : 0f);
        iceDamage = new Attribute(source != null && source.iceDamage != null ? source.iceDamage.Value : 0f);
        lightningDamage = new Attribute(source != null && source.lightningDamage != null ? source.lightningDamage.Value : 0f);
    }
}
