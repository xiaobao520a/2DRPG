using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AttackState : EnemyState
{
    public Enemy_AttackState(string stateName, StateMachine stateMachine, Entity entity) : base(stateName, stateMachine, entity)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //攻击的时候别动 别滑步
        rb.velocity = Vector2.zero;
    }

    
    public override void OnAnimationEvent(string eventName)
    {
        switch (eventName)
        {
            case "Enemy_Skeleton_AttackHit":
                Debug.Log("Enemy 攻击击中");
                break;

                //如果结束的时候发现还能砍到player 那就继续砍
            case "Enemy_Skeleton_AttackEnd":
                    stateMachine.ChangeState(enemy.battleState);
                break;
        }
    }
}
