using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_GroundState : EnemyState
{
    public Enemy_GroundState(string stateName, StateMachine stateMachine, Entity entity) : base(stateName, stateMachine, entity)
    {
    }

    public override void Update()
    {
        base.Update();

        //如果检测到了Player 就进入battleState
        if(enemy.DetectPlayer(ref enemy.playerHit))
        {
            stateMachine.ChangeState(enemy.battleState);
            return;
        }

    }
}
