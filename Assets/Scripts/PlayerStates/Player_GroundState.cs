using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//超状态 地面状态 在这里面检测跳跃 move和idle都可以直接检测到
public class Player_GroundState : PlayerState
{
    public Player_GroundState(string stateName, StateMachine stateMachine, Entity entity) : base(stateName, stateMachine, entity)
    {
    }

    public override void Update()
    {
        base.Update();

        //每帧检测跳跃是否按下并且接地 这样就跳跃
        if (player.playerInputSet.Player.Jump.WasPressedThisFrame()&&player.isGround)
        {
            stateMachine.ChangeState(player.JumpState);
            return;
        }

        //每帧检测是否按下攻击键 按下就进入普攻状态
        if (player.playerInputSet.Player.BasicAttack.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.BasicAttackState);
            return;
        }
    }
}
