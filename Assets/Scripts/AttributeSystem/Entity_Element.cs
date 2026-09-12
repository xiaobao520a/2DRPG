using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//专门控制元素效果的脚本 用来控制冰 火 电
public class Entity_Element : MonoBehaviour
{
    [SerializeField] private ElementDataSO elementDataSo;

    //正处于的元素状态
    public E_ElementType type;

    //应用元素效果的协程
    private Coroutine co_ApplyElementEffect;

    [Header("冰元素")]
    public float iceDuration; //持续时间
    public float slowDownMoveSpeed_Multiplier; //减慢移动速度的乘数
    public float slowDownAnimationSpeed_Multiplier; //减慢动画速度的乘数

    [Header("火元素")]
    public float fireDuration; //持续时间
    public float burnTickInterval; //每多少秒燃烧一次 造成一次伤害
    public float burnTickDamage; //每次燃烧的伤害

    private void Awake()
    {
        co_ApplyElementEffect = null;

        iceDuration = elementDataSo.iceDuration;
        slowDownMoveSpeed_Multiplier = elementDataSo.slowDownMoveSpeed_Multiplier;
        slowDownAnimationSpeed_Multiplier = elementDataSo.slowDownAnimationSpeed_Multiplier;

        fireDuration = elementDataSo.fireDuration;
        burnTickInterval= elementDataSo.burnTickInterval;
        burnTickDamage= elementDataSo.burnTickDamage;
    }

    //得到每种元素效果的默认持续时间
    public float GetElementDuration(E_ElementType elementType)
    {
        switch (elementType)
        {
            case E_ElementType.ice: return iceDuration;
            case E_ElementType.fire: return fireDuration;
            default: return 0f;
        }
    }

    //应用元素效果 冰减速 火燃烧
    public void ApplyElementEffect(Entity hurtEntity,float duration)
    {
        if (co_ApplyElementEffect != null) return; //如果已经有元素效果了 直接return 或者改成
        //同类型的元素效果return 如果是新的那就覆盖之类的 先保留

        //根据元素类型决定行为
        switch (type)
        {
            case E_ElementType.ice:
                co_ApplyElementEffect = StartCoroutine(ApplyIceEffect_Coroutine(hurtEntity,duration));
                break;

            case E_ElementType.fire:
                co_ApplyElementEffect = StartCoroutine(ApplyFireEffect_Coroutine(hurtEntity, duration,burnTickInterval,burnTickDamage));
                break;
        }
    }

    //冰冻
    private IEnumerator ApplyIceEffect_Coroutine(Entity hurtEntity,float duration)
    {
        float originalMoveSpeed = hurtEntity.moveSpeed;
        float originalBattleSpeed = 0; //Enemy才有battleSpeed Player没有
        float originalAnimatorSpeed = hurtEntity.animator.speed;

        //减速 动画和移动速度
        hurtEntity.moveSpeed *= slowDownMoveSpeed_Multiplier;
        hurtEntity.animator.speed *= slowDownAnimationSpeed_Multiplier;

        if (hurtEntity is Enemy)
        {
            originalBattleSpeed = (hurtEntity as Enemy).battleSpeed;
            (hurtEntity as Enemy).battleSpeed *= slowDownAnimationSpeed_Multiplier;
        }

        yield return new WaitForSeconds(duration);
        
        //结束后恢复原本速度
        hurtEntity.moveSpeed=originalMoveSpeed;
        hurtEntity.animator.speed = originalAnimatorSpeed;
        if (hurtEntity is Enemy) (hurtEntity as Enemy).battleSpeed = originalBattleSpeed;
        co_ApplyElementEffect = null;
    }

    //燃烧
    private IEnumerator ApplyFireEffect_Coroutine(Entity hurtEntity,float duration,float interval,float tickDamage)
    {
        float timer = 0f;

        while (timer < duration)
        {
            hurtEntity.ReduceHp(tickDamage);
            yield return new WaitForSeconds(interval);
            timer += interval;
        }

        co_ApplyElementEffect=null;
    }
}
