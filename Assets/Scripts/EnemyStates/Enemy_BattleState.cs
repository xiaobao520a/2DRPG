using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//战斗状态
public class Enemy_BattleState : EnemyState
{
    private int direction; //应该攻击的方向
    private float distance; //距离
    private float attackCDTimer; //攻击CD的计时器
    public Enemy_BattleState(string stateName, StateMachine stateMachine, Entity entity) : base(stateName, stateMachine, entity)
    {
    }

    public override void Enter()
    {
        base.Enter();
        attackCDTimer = 0;
        //一进入battleState就得到Player 方便后面追踪
        if(enemy.player==null) enemy.player=enemy.playerHit.transform.GetComponent<Player>();
    }
    public override void Update()
    {
        base.Update();

        attackCDTimer += Time.deltaTime;
        //一直监测Player 如果一段时间内没找到 就退出这个battleState回到idle
        if (!enemy.DetectPlayer(ref enemy.playerHit))
        {
            timer += Time.deltaTime;
            if(timer>enemy.maxBattleTime)
            {
                enemy.player = null; //回到idle的时候 把player的引用置空 找不到player才对
                stateMachine.ChangeState(enemy.idleState);
                return;
            }
        }
        else 
            timer = 0;

        //计算距离和方向
        CalculateDistanceAndDirection();

        //如果距离到攻击范围并且冷却时间到了 就攻击 同时计算一下需不需要转身之类的
        if(distance<=enemy.attackDistance&&attackCDTimer>enemy.attackCD)
        {
            if ((direction == 1 && !enemy.isRight) || (direction == -1 && enemy.isRight))
                enemy.SetFlip();

            stateMachine.ChangeState(enemy.attackState);
            return;
        }

        //追踪Player
        TrackPlayer();
    }

    private void TrackPlayer()
    {
        if ((direction == 1 && !enemy.isRight) || (direction == -1 && enemy.isRight))
            enemy.SetFlip();

        rb.velocity = new Vector2(enemy.battleSpeed*direction, rb.velocity.y);
    }

    //计算Player和Enemy的距离和方向
    private void CalculateDistanceAndDirection()
    {
        //距离
        distance =Vector2.Distance(enemy.player.transform.position,enemy.transform.position);
        if (enemy.player.transform.position.x - enemy.transform.position.x > 0)
            direction = 1;
        else if(enemy.player.transform.position.x - enemy.transform.position.x < 0)
            direction = -1;
        else
            direction = 0;
    }
}
