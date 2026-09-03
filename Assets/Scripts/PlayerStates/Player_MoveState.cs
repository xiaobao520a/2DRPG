using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Player的移动状态
public class Player_MoveState : Player_GroundState
{
    public Player_MoveState(string stateName, StateMachine stateMachine, Entity entity) : base(stateName, stateMachine, entity)
    {
    }

    public override void Update()
    {
        base.Update();

        //base里可能已经切走了(跳跃/攻击/格挡/dash) 切走了就不再执行本状态的速度逻辑
        if (stateMachine.CurrentState != this) return;

        //只要moveInput的x为0 就切换去idlestate
        if (player.moveInput.x == 0)
        {
            stateMachine.ChangeState(player.IdleState);
            return;
        }

        //如果移动的过程中撞墙了 方向也与墙一致 就切换回idle
        if(player.isWall&&((player.isRight&&player.moveInput.x>0)||(!player.isRight&&player.moveInput.x<0)))
        {
            stateMachine.ChangeState(player.IdleState);
            return;
        }

        //如果走到空中 就切换成FallState
        if(!player.isGround)
        {
            stateMachine.ChangeState(player.FallState);
            return;
        }

        //处理水平转向
        player.SetFlip();

        //设置速度
        rb.velocity = new Vector2(player.moveInput.x * player.moveSpeed, rb.velocity.y);

        
    }

    public override void Exit()
    {
        base.Exit();
    }
}
