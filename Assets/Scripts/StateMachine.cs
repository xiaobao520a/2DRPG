using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//状态机 用来控制各种状态的类
public class StateMachine
{
    private BaseState currentState;
    public BaseState CurrentState => currentState; //当前状态

    //初始化状态机 初始状态的方法
    public void Init(BaseState state)
    {
        currentState = state;
        currentState.Enter();
    }

    //改变状态
    public void ChangeState(BaseState state)
    {
        //如果状态相同 或者传入的状态是null 就return
        if (state == null||state==currentState) return;

        currentState?.Exit();
        currentState = state;
        currentState?.Enter();
    }

    //状态机一直运行的函数
    public void Update()
    {
        currentState.Update();
    }
}
