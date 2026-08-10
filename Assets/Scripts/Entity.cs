using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//实体 Player Enemy等 的基类
public abstract class Entity:MonoBehaviour
{
    protected StateMachine stateMachine;

    //初始化变量 组件
    protected virtual void Awake()
    {
        stateMachine = new StateMachine();
    }

    //一直执行状态机的Update
    protected virtual void Update()
    {
        stateMachine.Update();
    }
}
