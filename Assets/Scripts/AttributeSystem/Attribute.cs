using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//属性
[Serializable]
public class Attribute
{
    [SerializeField] private float value; //基础数值
    [SerializeField] private float buffValue; //buff数值

    public float Value => value+buffValue; //返回给外部的属性

    public Attribute()
    {
    }
    public Attribute(float initialValue)
    {
        value = initialValue;
    }

    //设置基础数值
    public void Set(float newValue)
    {
        value = newValue;
    }

    //添加buff
    public void AddABuff(string name,E_AttributeType type,float value)
    {

    }

    //删除Buff
    public void RemoveBuff(string name)
    {
    }

    public void ApplyBuff()
    {
    }
}
