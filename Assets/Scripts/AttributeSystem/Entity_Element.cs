using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//专门控制元素效果的脚本 用来控制冰 火 电
public class Entity_Element : MonoBehaviour
{
    [SerializeField] private ElementDataSO elementDataSo;

    //正处于的元素状态
    public E_ElementType nowType;

    [Header("冰元素")]
    public float iceDuration; //持续时间
    public float slowDownMoveSpeed_Multiplier; //减慢移动速度的乘数
    public float slowDownAnimationSpeed_Multiplier; //减慢动画速度的乘数

    private void Awake()
    {
        iceDuration=elementDataSo.iceDuration;
        slowDownMoveSpeed_Multiplier = elementDataSo.slowDownMoveSpeed_Multiplier;
        slowDownAnimationSpeed_Multiplier = elementDataSo.slowDownAnimationSpeed_Multiplier;

    }

    //应用元素效果 冰减速
    public void ApplyElementEffect(HurtData hurtData)
    {
        float originalMoveSpeed = hurtData.hurtEntity.moveSpeed;
        float originalBattleSpeed = 0;

        //减速
        hurtData.hurtEntity.moveSpeed *= slowDownMoveSpeed_Multiplier;
        if (hurtData.hurtEntity is Enemy)
        {
            originalBattleSpeed = (hurtData.hurtEntity as Enemy).battleSpeed;
            (hurtData.hurtEntity as Enemy).battleSpeed *= slowDownAnimationSpeed_Multiplier;
        }
    }

}
