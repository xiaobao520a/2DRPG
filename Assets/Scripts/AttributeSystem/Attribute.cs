using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//属性
[Serializable]
public class Attribute
{
    [SerializeField] private float value; //基础数值

    public float Value=>value; //返回给外部的属性

}
