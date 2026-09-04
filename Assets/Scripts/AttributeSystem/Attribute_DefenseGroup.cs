using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//防御 闪避相关属性
[Serializable]
public class Attribute_DefenseGroup
{
    //物理防御
    public Attribute armor; //护甲
    public Attribute evasion; //闪避率

    //元素抗性
    public Attribute fireRes; //火抗性
    public Attribute iceRes; //冰抗性
    public Attribute lightningRes; //闪电抗性

}
