using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : BaseState
{
    protected Enemy enemy;
    public EnemyState(string stateName, StateMachine stateMachine, Entity entity) : base(stateName, stateMachine, entity)
    {
        enemy= (Enemy)entity;
    }

    public override void Update()
    {
        base.Update();

        //一直设置move的动画速度参数 和battle的混合树的动画参数
        animator.SetFloat("MoveAnimSpeedMultiplier", enemy.moveAnimSpeedMultiplier);
        animator.SetFloat("BattleXVelocity",rb.velocity.x);
    }
}
