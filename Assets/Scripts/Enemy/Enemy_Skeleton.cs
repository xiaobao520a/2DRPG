using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skeleton : Enemy
{
    [Header("配置文件")]
    public Enemy_SkeletonDataSO enemy_SkeletonDataSO;

    protected override void Awake()
    {
        //得到通用的组件
        base.Awake();

        //初始化所有状态
        idleState = new Enemy_IdleState("Idle",stateMachine,this);
        moveState = new Enemy_MoveState("Move", stateMachine, this);
        attackState = new Enemy_AttackState("Attack", stateMachine, this);
        battleState = new Enemy_BattleState("Battle", stateMachine, this);
        dieState = new Enemy_DieState("Die", stateMachine, this);
        stunnedState = new Enemy_StunnedState("Stunned", stateMachine, this);
        

        //初始化变量
        idleTime = enemy_SkeletonDataSO.idleTime;
        moveSpeed= enemy_SkeletonDataSO.moveSpeed;
        detectPlayer_Distance=enemy_SkeletonDataSO.detectPlayer_Distance;
        playerHit = default;
        maxBattleTime=enemy_SkeletonDataSO.maxBattleTime;
        attackDistance=enemy_SkeletonDataSO.attackDistance;
        battleSpeed=enemy_SkeletonDataSO.battleSpeed;
        attackCD=enemy_SkeletonDataSO.attackCD;
        nowHp=enemy_SkeletonDataSO.nowHp;
        maxHp=enemy_SkeletonDataSO.maxHp;
        isDead = false;
        attackDamage=enemy_SkeletonDataSO.attackDamage;
        attackRadius=enemy_SkeletonDataSO.attackRadius;
        attackAngle=enemy_SkeletonDataSO.attackAngle;
        attackOffset=enemy_SkeletonDataSO.attackOffset;
        knockBackForce=enemy_SkeletonDataSO.knockBackForce;
        stunnedDuration=enemy_SkeletonDataSO.stunnedDuration;
    }

    protected override void Update()
    {
        base.Update();

        
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(transform.position, transform.position + new Vector3((isRight ? 1 : -1) * attackDistance, 0, 0));

    }
}
