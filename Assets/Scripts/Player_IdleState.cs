using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Player的空闲状态
public class Player_IdleState : PlayerState
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

        //检测是否在移动
        if (player.moveInput.x != 0)
        {
            stateMachine.ChangeState(player.MoveState);
            return;
        }
        
    }
}
