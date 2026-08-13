using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_FallState : Player_AirState
{
    public Player_FallState(string stateName, StateMachine stateMachine, Entity entity) : base(stateName, stateMachine, entity)
    {
    }

    public override void Update()
    {
        base.Update();

        //如果接地了 就切换成IdleState 并且把animator的参数JumpFall改成false
        if (player.isGround)
        {
            stateMachine.ChangeState(player.IdleState);
            return;
        }


    }
    
}
