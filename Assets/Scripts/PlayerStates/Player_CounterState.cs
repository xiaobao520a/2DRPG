using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_CounterState : PlayerState
{
    public Player_CounterState(string stateName, StateMachine stateMachine, Entity entity) : base(stateName, stateMachine, entity)
    {
    }

    public override void Update()
    {
        base.Update();

        timer += Time.deltaTime;
        if(timer>player.counterDuration)
        {
            stateMachine.ChangeState(player.IdleState);
            return;
        }
    }
}
