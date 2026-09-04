using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//属性
[Serializable]
public class Attribute
{
    [SerializeField] private float value; //数值

    public Attribute()
    {
    }

    public Attribute(float initialValue)
    {
        value = initialValue;
    }

    public float Value => value; //返回给外部的属性

    //设置数值 运行时加点 Buff 用
    public void Set(float newValue)
    {
        value = newValue;
    }
}
