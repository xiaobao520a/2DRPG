using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_WallJumpState : PlayerState
{
    private int wallJumpDirection; //离墙方向（Enter时锁定）：1=向右 -1=向左，防止跳跃中转向导致推力反向

    public Player_WallJumpState(string stateName, StateMachine stateMachine, Entity entity) : base(stateName, stateMachine, entity)
    {
    }

    public override void Enter()
    {
        //重写一下Enter的逻辑 因为Animator里没有这个WallJump的animation 直接复用JumpFall的就行
        animator.SetBool("JumpFall", true);
        timer = 0;
        wallJumpDirection = player.isRight ? -1 : 1; //进入时锁定离墙方向，之后SetFlip不影响推力
        rb.velocity = new Vector2(player.wallJumpForce.x * wallJumpDirection, player.wallJumpForce.y);
    }

    public override void Update()
    {
        base.Update();

        //可以转向
        player.SetFlip();
         
        //离墙推力使用 Enter 时锁定的方向，水平输入只做小幅修正（上限为初始推力的50%）
        //避免不按方向键时推力归零，也避免 SetFlip 转向后推力跟着反向、被弹回墙面
        float basePushX = player.wallJumpForce.x * wallJumpDirection;
        float steerX = player.moveInput.x * player.wallJumpForce.x * player.inAir_Multiplier * 0.5f;
        rb.velocity = new Vector2(basePushX + steerX, rb.velocity.y);

        
        timer += Time.deltaTime;
        if(timer>player.wallJumpTime)
        {
            stateMachine.ChangeState(player.FallState);
            return;
        }
    }

    //重写Exit的逻辑 让它改Animator的参数JumpFall 这里主要是为了wallJump的时候dash animator能正常切换
    public override void Exit()
    {
        animator.SetBool("JumpFall", false);
    }
}
