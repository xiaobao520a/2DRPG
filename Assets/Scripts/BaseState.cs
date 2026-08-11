using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//基类状态 抽象类 
public abstract class BaseState
{
    protected string stateName; //状态名 让它与animator中的参数名一样 这样方便控制动画
    protected StateMachine stateMachine; //控制该状态的状态机
    protected Entity entity; //该状态属于的 具体的实体(Player Enemy...)
    protected Animator animator; //entity的Animator组件
    protected Rigidbody2D rb; //rigidBody2D组件
    protected float timer; //状态的计时器 需要的时候 需要的状态自己赋值去管理

    public BaseState(string stateName, StateMachine stateMachine,Entity entity)
    {
        this.stateMachine = stateMachine;
        this.stateName = stateName;
        this.entity = entity;

        animator=entity.GetComponentInChildren<Animator>();
        rb= entity.GetComponentInChildren<Rigidbody2D>();
    }

    //进入状态的函数
    public virtual void Enter()
    {
        //进入的时候就开启该状态的动画
        animator.SetBool(stateName, true);
    }

    //处于该状态 的函数
    public virtual void Update()
    {
    }

    //退出该状态 的函数
    public virtual void Exit()
    {
        //退出的时候就退出该状态的动画
        animator.SetBool(stateName, false);
    }

}
