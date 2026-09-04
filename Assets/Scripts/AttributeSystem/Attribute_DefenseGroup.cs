using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//防御 闪避相关属性
[Serializable]
public class Attribute_DefenseGroup
{
    public Attribute armor = new Attribute(0); //护甲
    public Attribute evasion = new Attribute(0); //闪避率基础值(%)

    //元素抗性
    public Attribute fireRes = new Attribute(0); //火抗
    public Attribute iceRes = new Attribute(0); //冰抗
    public Attribute lightningRes = new Attribute(0); //闪电抗

    public Attribute_DefenseGroup()
    {
    }

    //深拷贝构造函数
    public Attribute_DefenseGroup(Attribute_DefenseGroup source)
    {
        armor = new Attribute(source != null && source.armor != null ? source.armor.Value : 0f);
        evasion = new Attribute(source != null && source.evasion != null ? source.evasion.Value : 0f);
        fireRes = new Attribute(source != null && source.fireRes != null ? source.fireRes.Value : 0f);
        iceRes = new Attribute(source != null && source.iceRes != null ? source.iceRes.Value : 0f);
        lightningRes = new Attribute(source != null && source.lightningRes != null ? source.lightningRes.Value : 0f);
    }
}
