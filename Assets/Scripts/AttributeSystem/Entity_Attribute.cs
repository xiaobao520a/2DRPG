using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//挂载在Entity上的 属性脚本
public class Entity_Attribute : MonoBehaviour
{
    private Entity entity; //Entity脚本

    public Attribute_MajorGroup majorGroup; //主要属性 只有Player拥有 力量 敏捷 智力 活力
    public Attribute_AttackGroup attackGroup; //攻击属性 分物理攻击 元素攻击
    public Attribute_DefenseGroup defenseGroup; //防御属性 护甲 闪避率 元素抗性

    private void Awake()
    {
        entity = GetComponent<Entity>();
    }

    //得到最大血量
    public float GetMaxHp()
    {
        float baseHp=entity.maxHp;
        float bonusHp = majorGroup.vitality.Value * 5f; //每一点活力值增加5点最大HP
        return baseHp + bonusHp;
    }

    
}
