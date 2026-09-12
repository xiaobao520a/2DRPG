using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//属性类型枚举: 装备/Buff等加成的"加成目标"清单
//给 Entity_Attribute.ApplyModifier / RemoveModifier 做 switch 用
public enum E_AttributeType
{
    //===== 主属性 (Attribute_MajorGroup)
    Strength,     //力量          -> majorGroup.strength
    Agility,      //敏捷          -> majorGroup.agility
    Intelligence, //智力          -> majorGroup.intelligence
    Vitality,     //活力          -> majorGroup.vitality

    //===== 攻击属性 (Attribute_AttackGroup)
    PhysicalDamage,   //物理攻击      -> attackGroup.damage
    CritChance,       //暴击率(%)     -> attackGroup.critChance
    CritPower,        //暴击伤害(%)   -> attackGroup.critPower
    ArmorPenetration, //护甲穿透(%)   -> attackGroup.armorPenetration
    FireDamage,       //火焰伤害      -> attackGroup.fireDamage
    IceDamage,        //冰冻伤害      -> attackGroup.iceDamage
    LightningDamage,  //闪电伤害      -> attackGroup.lightningDamage

    //===== 防御属性 (Attribute_DefenseGroup) =====
    Armor,            //护甲          -> defenseGroup.armor
    Evasion,          //闪避率(%)     -> defenseGroup.evasion
    FireRes,          //火焰抗性(%)   -> defenseGroup.fireRes
    IceRes,           //冰冻抗性(%)   -> defenseGroup.iceRes
    LightningRes,     //闪电抗性(%)   -> defenseGroup.lightningRes

    //===== 派生属性 =====
    MaxHp,            //最大生命值    -> 不在组里 由GetMaxHp算 需特殊处理
}
