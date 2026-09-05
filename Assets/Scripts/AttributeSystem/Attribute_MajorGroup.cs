using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//主要属性 只有Player拥有
[Serializable]
public class Attribute_MajorGroup
{
    public Attribute strength = new Attribute(5); //力量
    public Attribute agility = new Attribute(5); //敏捷
    public Attribute intelligence = new Attribute(5); //智力
    public Attribute vitality = new Attribute(5); //活力 每点活力增加 vitalityToHp 点最大HP

    public Attribute_MajorGroup()
    {
    }

    //深拷贝构造函数 从模板复制一份独立的运行时数据
    public Attribute_MajorGroup(Attribute_MajorGroup source)
    {
        strength = new Attribute(source != null && source.strength != null ? source.strength.Value : 0f);
        agility = new Attribute(source != null && source.agility != null ? source.agility.Value : 0f);
        intelligence = new Attribute(source != null && source.intelligence != null ? source.intelligence.Value : 0f);
        vitality = new Attribute(source != null && source.vitality != null ? source.vitality.Value : 0f);
    }

    public Attribute_MajorGroup(float strength,float agility,float intelligence,float vitality)
    {
        this.strength.Set(strength);
        this.agility.Set(agility);
        this.intelligence.Set(intelligence);
        this.vitality.Set(vitality);  

    }

}
