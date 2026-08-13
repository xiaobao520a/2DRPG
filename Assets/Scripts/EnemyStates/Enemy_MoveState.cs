using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_MoveState : Enemy_GroundState
{
    public Enemy_MoveState(string stateName, StateMachine stateMachine, Entity entity) : base(stateName, stateMachine, entity)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //进入的时候如果已经在边缘了 那么就转向
        if(!enemy.isGround||enemy.isWall)
            enemy.SetFlip();
    }
    public override void Update()
    {
        base.Update();

        //设置速度
        rb.velocity = new Vector2(enemy.moveSpeed * (enemy.isRight ? 1 : -1), rb.velocity.y);

        //如果走到边缘了或者撞墙了 那就进入idleState停一会 再转向去另一边走
        if(!enemy.isGround||enemy.isWall)
        {
            stateMachine.ChangeState(enemy.idleState);
            return;
        }

    }
}
