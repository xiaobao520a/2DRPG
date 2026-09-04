using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//主要属性 只有Player拥有
[Serializable]
public class Attribute_MajorGroup
{
    public Attribute strength; //力量
    public Attribute agility; //敏捷
    public Attribute intelligence; //智力
    public Attribute vitality; //活力 每一点活力值增加5点最大HP
}
