using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_WallJumpState : PlayerState
{
    public Player_WallJumpState(string stateName, StateMachine stateMachine, Entity entity) : base(stateName, stateMachine, entity)
    {
    }

    public override void Enter()
    {
        //重写一下Enter的逻辑 因为Animator里没有这个WallJump的animation 直接复用JumpFall的就行
        animator.SetBool("JumpFall", true);

        rb.velocity = new Vector2(player.wallJumpForce.x * (player.isRight ? -1 : 1), player.wallJumpForce.y);
    }

    public override void Update()
    {
        base.Update();

        //如果竖直速度开始向下 就切换成FallState
        if(rb.velocity.y<0)
        {
            stateMachine.ChangeState(player.FallState);
            return;
        }
    }

    //重写Exit的逻辑 让它不要改Animator的参数 
    public override void Exit()
    {
    }
}
