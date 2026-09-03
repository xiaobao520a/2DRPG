using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//敌人被击晕状态 也就是被player反击打中的状态
public class Enemy_StunnedState : EnemyState
{
    public Enemy_StunnedState(string stateName, StateMachine stateMachine, Entity entity) : base(stateName, stateMachine, entity)
    {
    }

    public override void Enter()
    {
        base.Enter();

    }

    public override void Update()
    {
        base.Update();

        //如果超过了击晕的持续时间就回到idleState
        timer += Time.deltaTime;
        if(timer>enemy.stunnedDuration)
        {
            stateMachine.ChangeState(enemy.idleState);
            return;
        }
    }
}
