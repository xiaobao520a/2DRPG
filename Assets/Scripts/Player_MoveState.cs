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

        //处理水平转向
        player.SetFlip();

        //设置速度
        rb.velocity = new Vector2(player.moveInput.x * player.moveSpeed, rb.velocity.y);

        
    }

}
