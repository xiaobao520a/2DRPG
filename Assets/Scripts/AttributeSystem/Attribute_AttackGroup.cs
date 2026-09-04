using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//攻击相关属性
[Serializable]
public class Attribute_AttackGroup
{
    //物理攻击
    public Attribute damage;
    public Attribute critPower;
    public Attribute critChance;

    //元素攻击
    public Attribute fireDamage;
    public Attribute iceDamage;
    public Attribute lightningDamage;
}
