using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_JumpState : Player_AirState
{
    public Player_JumpState(string stateName, StateMachine stateMachine, Entity entity) : base(stateName, stateMachine, entity)
    {

    }

    public override void Enter()
    {
        base.Enter();

        //设置跳跃速度
        rb.velocity = new Vector2(rb.velocity.x, player.jumpForce);
    }

    public override void Update()
    {
        base.Update();

        //如果y的速度<0 就下降
        if(rb.velocity.y<0)
        {
            stateMachine.ChangeState(player.FallState);
            return;
        }

    }

}
