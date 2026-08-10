using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//基类状态 抽象类 
public abstract class BaseState
{
    protected string stateName; //状态名
    protected StateMachine stateMachine; //控制该状态的状态机
    protected Entity entity; //该状态属于的 具体的实体(Player Enemy...)

    public BaseState(string stateName, StateMachine stateMachine,Entity entity)
    {
        this.stateMachine = stateMachine;
        this.stateName = stateName;
        this.entity = entity;
    }

    //进入状态的函数
    public virtual void Enter()
    {
    }

    //处于该状态 的函数
    public virtual void Update()
    {
    }

    //退出该状态 的函数
    public virtual void Exit()
    {
    }

}
