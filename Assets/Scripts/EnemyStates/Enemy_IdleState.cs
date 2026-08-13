using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_IdleState : Enemy_GroundState
{
    public Enemy_IdleState(string stateName, StateMachine stateMachine, Entity entity) : base(stateName, stateMachine, entity)
    {
    }

    public override void Enter()
    {
        base.Enter();

        rb.velocity = Vector2.zero;
    }

    public override void Update()
    {
        base.Update();

        //如果超过了Idle状态的最大持续时间 就切换成moveState
        timer += Time.deltaTime;
        if (timer > enemy.idleTime)
        {
            stateMachine.ChangeState(enemy.moveState);
            return;
        }
    }
}
