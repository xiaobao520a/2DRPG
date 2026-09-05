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

    //从配置文件深拷贝初始化属性组 运行时只改这里的数据 不污染SO资产
    public void InitAttributes(PlayerDataSO so)
    {
        majorGroup = new Attribute_MajorGroup(so.majorGroup);
        attackGroup = new Attribute_AttackGroup(so.attackGroup);
        defenseGroup = new Attribute_DefenseGroup(so.defenseGroup);
    }

    public void InitAttributes(EnemyDataSO so)
    {
        attackGroup = new Attribute_AttackGroup(so.attackGroup);
        defenseGroup = new Attribute_DefenseGroup(so.defenseGroup);
    }

    //得到最大血量 初始化的时候用
    public float GetMaxHp(float vitalityToHp)
    {
        float baseHp = entity.maxHp;
        float bonusHp = majorGroup.vitality.Value * vitalityToHp;
        return baseHp + bonusHp;
    }

    //得到闪避率 限制闪避率有一个最大值
    public float GetEvasion(float agilityToEvasion, float maxEvasion)
    {
        float baseEvasion = defenseGroup.evasion.Value;
        float bonusEvasion = majorGroup.agility.Value * agilityToEvasion;
        float finalEvasion = Mathf.Clamp(baseEvasion + bonusEvasion, 0, maxEvasion);

        return finalEvasion;
    }

    //得到物理伤害 1 0.3 0.5 返回是否暴击
    public float GetPhysicalDamage(float strengthToDamage,float agilityToCritChance,float strengthToCritPower,out bool isCrit)
    {
        //基础伤害(力量加成)
        float baseDamage=attackGroup.damage.Value;
        float bonusDamage = majorGroup.strength.Value * strengthToDamage;
        float totalBaseDamage=baseDamage + bonusDamage;

        //暴击率
        float baseCritChance=attackGroup.critChance.Value;
        float bonusCritChance=majorGroup.agility.Value*agilityToCritChance;
        float critChance=baseCritChance+bonusCritChance;

        //暴击伤害(相当于是攻击的Multiplier)
        float baseCritPower=attackGroup.critPower.Value;
        float bonusCritPower = majorGroup.strength.Value * strengthToCritPower;
        float critPower = (baseCritPower + bonusCritPower) / 100;

        //如果暴击 就乘上暴击伤害
        isCrit = Random.Range(0, 100) < critChance;
        float finalDamage=isCrit?totalBaseDamage*critPower:totalBaseDamage;
        return finalDamage;

    }
}
