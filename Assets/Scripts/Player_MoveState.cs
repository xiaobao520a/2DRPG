using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Player的移动状态
public class Player_MoveState : PlayerState
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

        //处理水平翻转
        if ((player.isRight && player.moveInput.x < 0) || (!player.isRight && player.moveInput.x > 0))
            player.Flip();

        //设置速度
        rb.velocity = new Vector2(player.moveInput.x * player.moveSpeed, rb.velocity.y);

        
    }

}
