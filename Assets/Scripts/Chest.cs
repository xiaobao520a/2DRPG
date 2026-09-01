using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//可破坏的箱子
public class Chest : MonoBehaviour, IDamageable
{
    private Animator animator;
    private bool isOpen = false; 
    private void Awake()
    {
        animator= GetComponentInChildren<Animator>();
    }

    //受伤就打开箱子 播放特效
    public void TakeDamage(AttackHitData hitData)
    {
        //箱子只能打开一次 如果打开过了 就不执行后面的逻辑了
        if (isOpen) return;

        animator.SetBool("Open", true);
        EventCenter.Instance.Broadcast<Chest>(E_EventType.ChestOpen,this);
        isOpen = true;
    }
}
