using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_WallSlideState : PlayerState
{
    public Player_WallSlideState(string stateName, StateMachine stateMachine, Entity entity) : base(stateName, stateMachine, entity)
    {
    }

    public override void Update()
    {
        base.Update();

        //如果滑到接地了 就切换会idle
        if (player.isGround)
        {
            stateMachine.ChangeState(player.IdleState);
            return;
        }

        //如果在墙上滑行的时候按下了跳跃键 就进入WallJumpState
        if(player.playerInputSet.Player.Jump.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.WallJumpState);
            return;
        }

        //如果在墙上滑行的时候 按下了相反的AD键 就进入FallState 
        if ((player.isRight && player.moveInput.x < 0) || (!player.isRight && player.moveInput.x > 0))
        {

            stateMachine.ChangeState(player.FallState);
            return;
        }

        //按下S 下滑速度更快 不按或者按其他键的话 就慢下滑
        if (player.moveInput.y < 0)
            rb.velocity = new Vector2(rb.velocity.x, player.wallSlideSpeed*-1);
        else
            rb.velocity = new Vector2(rb.velocity.x, player.wallSlideSpeed * -1 * player.inWall_Multiplier);
    }
}
