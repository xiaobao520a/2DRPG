using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ParryState : PlayerState
{
    //范围检测可反击物体的碰撞器
    private Collider2D[] colliders;

    //反击的数据
    private AttackHitData hitData;

    //可被反击的接口
    private ICountered iCountered;
    public Player_ParryState(string stateName, StateMachine stateMachine, Entity entity) : base(stateName, stateMachine, entity)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //进入格挡状态的时候把速度设为0
        rb.velocity = Vector2.zero;

        //反击的数据 沿用攻击hit数据 反击也算攻击
        hitData = new AttackHitData
        {
            damage = player.counterDamage,
            knockBackForce = player.counterKnockBackForce,
            knockBackDirection = player.isRight ? 1 : -1,
            hitEntity = player
        };
    }
    public override void Update()
    {
        base.Update();

        timer += Time.deltaTime;

        //范围检测 检测parry附近的 有ICountered的物体
        Vector2 startPoint = (Vector2)player.transform.position + player.parryDetect_Offset * (player.isRight ? 1 : -1);
        colliders = Physics2D.OverlapCircleAll(startPoint, player.parryDetect_Radius);

        Vector2 facingDirection = player.isRight ? Vector2.right : Vector2.left;
        foreach (Collider2D collider in colliders)
        {
            iCountered = collider.GetComponent<ICountered>();
            //如果没有ICountered接口 就不管
            if (iCountered== null) continue;
            if (!iCountered.CanBeCountered) continue; //如果不在反击窗口 也不管

            //扇形判定：只命中面朝方向 parryDetect_Angle 度内的目标（视为完整扇区角，默认120=面前±60度）
            Vector2 directionToTarget = (Vector2)collider.transform.position - startPoint;
            if (Vector2.Angle(facingDirection, directionToTarget) > player.parryDetect_Angle / 2f)
                continue;

            //切换反击状态并且击中敌人
                iCountered.Countered(hitData);

            stateMachine.ChangeState(player.CounterState);
            return;
        }


        //如果超过了格挡持续时间 就回到idleState
        if (timer>player.parryDuration)
        {
            stateMachine.ChangeState(player.IdleState);
            return;
        }
    }
}
