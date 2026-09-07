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
        majorGroup = new Attribute_MajorGroup(0, 0, 0, 0);
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

    //得到物理伤害 返回是否暴击
    public float GetPhysicalDamage(float strengthToDamage, float agilityToCritChance, float strengthToCritPower, out bool isCrit)
    {
        //基础伤害(力量加成)
        float baseDamage = attackGroup.damage.Value;
        float bonusDamage = majorGroup.strength.Value * strengthToDamage;
        float totalBaseDamage = baseDamage + bonusDamage;

        //暴击率
        float baseCritChance = attackGroup.critChance.Value;
        float bonusCritChance = majorGroup.agility.Value * agilityToCritChance;
        float critChance = baseCritChance + bonusCritChance;

        //暴击伤害(相当于是攻击的Multiplier)
        float baseCritPower = attackGroup.critPower.Value;
        float bonusCritPower = majorGroup.strength.Value * strengthToCritPower;
        float critPower = (baseCritPower + bonusCritPower) / 100;

        //如果暴击 就乘上暴击伤害
        isCrit = Random.Range(0, 100) < critChance;
        float finalDamage = isCrit ? totalBaseDamage * critPower : totalBaseDamage;
        return finalDamage;
    }

    //得到元素伤害
    public float GetElementDamage(float intelligenceToElementDamage,out E_ElementType type)
    {
        float iceElementDamage = attackGroup.iceDamage.Value;
        float fireElementDamage = attackGroup.fireDamage.Value;
        float lightningElementDamage = attackGroup.lightningDamage.Value;
        float bonusElementDamage = majorGroup.intelligence.Value * intelligenceToElementDamage;

        float maxElementDamage = iceElementDamage;
        E_ElementType tempType = E_ElementType.ice;

        if (fireElementDamage > maxElementDamage)
        {
            maxElementDamage = fireElementDamage;
            tempType = E_ElementType.fire;
        }

        if (lightningElementDamage > maxElementDamage)
        {
            maxElementDamage = lightningElementDamage;
            tempType = E_ElementType.lightning;
        }

        if (maxElementDamage == 0)
        {
            tempType = E_ElementType.none;
            type = tempType;
            return 0;
        }

        float finalElementDamage = bonusElementDamage+maxElementDamage;

        if (maxElementDamage != iceElementDamage) 
            finalElementDamage += iceElementDamage*0.5f;

        if (maxElementDamage != fireElementDamage) 
            finalElementDamage += fireElementDamage*0.5f;

        if (maxElementDamage != lightningElementDamage) 
            finalElementDamage += lightningElementDamage*0.5f;

        type=tempType;
        return finalElementDamage;
    }

    //得到元素抗性率
    public float GetElementRes(float intelligenceToElementRes,E_ElementType type)
    {
        float baseRes = 0;
        switch(type)
        {
            case E_ElementType.ice:
                baseRes = defenseGroup.iceRes.Value;
                break;
            case E_ElementType.fire:
                baseRes = defenseGroup.fireRes.Value;
                break;
            case E_ElementType.lightning:
                baseRes = defenseGroup.lightningRes.Value;
                break;
        }

        float bonusRes = majorGroup.intelligence.Value * intelligenceToElementRes;
        float finalRes = Mathf.Clamp(bonusRes + baseRes, 0, entity.maxElementRes) / 100;
        return finalRes;
    }

    //得到护甲减伤率 传入攻击者的破甲率 破甲会先折算护甲再算减伤
    public float GetArmorMitigation(float agilityToArmor, float maxArmorMitigation, float armorPenetration)
    {
        float baseArmor = defenseGroup.armor.Value;
        float bonusArmor = majorGroup.agility.Value * agilityToArmor;
        float totalArmor = (baseArmor + bonusArmor) * Mathf.Clamp01(1f - armorPenetration); //破甲折算后的有效护甲

        float mitigation = totalArmor / (totalArmor + 100);
        mitigation = Mathf.Clamp(mitigation, 0, maxArmorMitigation);

        return mitigation;
    }

    //得到破甲率(0~1)
    public float GetArmorPenetration()
    {
        //先限制百分比在0~100 再转成0~1
        float armorPenetration = Mathf.Clamp(attackGroup.armorPenetration.Value, 0, 100) / 100f;
        return armorPenetration;
    }
}
