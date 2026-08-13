using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//冲刺状态
public class Player_DashState : PlayerState
{
    private float originalGravityScale;
    public Player_DashState(string stateName, StateMachine stateMachine, Entity entity) : base(stateName, stateMachine, entity)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //先暂时禁用rb的重力 防止在空中dash的时候 受重力影响下降 很奇怪
        originalGravityScale = rb.gravityScale;
        rb.gravityScale = 0;

        //设置dashState能持续的时间
        timer = player.dashTime;

        //设置冲刺的速度
        rb.velocity = new Vector2(player.dashSpeed * (player.isRight? 1 : -1), 0);
    }

    public override void Update()
    {
        base.Update();

        //很奇怪啊 有bug 只能强制在dashState里面每帧去锁定速度了
        rb.velocity = new Vector2(player.dashSpeed * (player.isRight ? 1 : -1), 0);


        timer -= Time.deltaTime;
        //如果冲到一半发现冲到了墙上 那就进入wallSlideState
        if (player.isWall)
        {
            stateMachine.ChangeState(player.WallSlideState);
            return;
        }

        if (timer <= 0)
        {
            //如果此时Player在地上 就变成idlestate
            if (player.isGround)
            {
                stateMachine.ChangeState(player.IdleState);
                return;
            }

            //如果此时Player在天上 直接变成FallSTATE
            else
            {
                stateMachine.ChangeState(player.FallState);
                return;
            }
        }

    }

    public override void Exit()
    {
        base.Exit();

        //退出的时候恢复rb的重力
        rb.gravityScale=originalGravityScale;

        //要等冷却时间过了之后才能继续dash
        player.canDash = false;
    }
}
