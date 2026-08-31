using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Player的空闲状态
public class Player_IdleState : Player_GroundState
{
    public Player_IdleState(string stateName, StateMachine stateMachine, Entity entity) : base(stateName, stateMachine, entity)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //进入该状态时 速度为0
        rb.velocity = new Vector2(0, 0);
    }

    public override void Update()
    {
        base.Update();

        //受击击退的剩余速度 逐渐衰减 防止站原地一直滑动
        if (rb.velocity.x != 0)
            rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, 0, player.knockBackDeceleration * Time.deltaTime), rb.velocity.y);


        //检测是否在墙边并且往墙的方向移动 这样的话就不进入moveState 保持Idle 防止动画一直闪烁
        if (player.isWall && ((player.isRight && player.moveInput.x > 0) || (!player.isRight && player.moveInput.x < 0)))
            return;

        //检测是否在移动
        if (player.moveInput.x != 0)
        {
            stateMachine.ChangeState(player.MoveState);
            return;
        }
        
    }
}
