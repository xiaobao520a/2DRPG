using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_BasicAttackState : PlayerState
{
    private float velocityTimer = 0; //每一段攻击 加多久速度的计时器
    private Collider2D[] colliders; //记录攻击范围检测到的 所有碰撞器
    public Player_BasicAttackState(string stateName, StateMachine stateMachine, Entity entity) : base(stateName, stateMachine, entity)
    {
    }

    private int basicAttackIndex; //第几段普攻的下标
    private bool isWindow; //是否是在普攻结束后的 可以检测输入的窗口
    private float windowTime; //窗口持续的时间
    public override void Enter()
    {
        //初始化第一次攻击
        base.Enter();

        velocityTimer = 0;
        basicAttackIndex = 0;
        animator.SetInteger("BasicAttackIndex", basicAttackIndex);

        //初始化各种变量
        isWindow = false;
        windowTime=player.basicAttack_TimeWindow;

        //设置攻击时player的速度
        rb.velocity = new Vector2(player.basicAttack_Velocity[basicAttackIndex].x * (player.isRight ? 1 : -1),
            player.basicAttack_Velocity[basicAttackIndex].y);
    }

    public override void Update()
    {
        base.Update();

        //在可以加速度的窗口内 设置速度 一旦超过这个时间 就别加速度了
        velocityTimer += Time.deltaTime;
        if (velocityTimer <= player.basicAttack_velocityTimeWindow)
        {
            //锁死速度水平速度 竖直不要 防止它飞起来
            float lockedHorizontalSpeed = player.basicAttack_Velocity[basicAttackIndex].x
                                           * (player.isRight ? 1 : -1);
            rb.velocity = new Vector2(lockedHorizontalSpeed, rb.velocity.y);
        }
        else
            rb.velocity = Vector2.zero;
        

        //如果现在是在输入窗口内
        if (isWindow)
        {
            //在输入窗口内的时候 把速度弄成0 防止它滑动
            rb.velocity = new Vector2(0, 0);
            timer += Time.deltaTime;
            //如果窗口时间内都没有输入普攻 就切换成idleState
            if (timer > windowTime)
            {
                stateMachine.ChangeState(player.IdleState);
                return;
            }

            //普攻被按下了 就执行下一次普攻
            if (player.playerInputSet.Player.BasicAttack.WasPressedThisFrame())
                PerformNextAttack();
        }
    }

    //具体实现普攻命中 和普攻结束逻辑的函数
    public override void OnAnimationEvent(string eventName)
    {
        switch (eventName)
        {
            case "BasicAttackHit":
                Vector2 startPoint = (Vector2)player.transform.position + player.attackOffset*(player.isRight?1:-1);
                colliders = Physics2D.OverlapCircleAll(startPoint, player.attackRadius, player.enemyLayer|player.chestLayer);

                AttackHitData hitData = new AttackHitData
                {
                    damage = player.attackDamage,
                    knockBackForce = player.knockBackForce,
                    knockBackDirection = player.isRight ? 1 : -1,
                    hitEntity =player
                };

                Vector2 facingDirection = player.isRight ? Vector2.right : Vector2.left;
                foreach (Collider2D collider in colliders)
                {
                    //扇形判定：只命中面朝方向 attackAngle 度内的目标（attackAngle 视为完整扇区角，默认120=面前±60度）
                    Vector2 directionToTarget = (Vector2)collider.transform.position - startPoint;
                    if (Vector2.Angle(facingDirection, directionToTarget) > player.attackAngle / 2f)
                        continue;

                    //击中目标
                    else
                    {
                        collider.GetComponent<IDamageable>()?.TakeDamage(hitData);
                    }
                }
                break;

            //普攻结束时 开启输入窗口 检测输入
            case "BasicAttackEnd":
                isWindow = true;
                break;
        }
    }

    //执行下一次攻击
    private void PerformNextAttack()
    {
        //重置输入窗口 和计时器
        isWindow = false;
        timer = 0f;
        velocityTimer = 0f;

        basicAttackIndex++;
        //目前只有三段攻击
        if (basicAttackIndex >= player.basicAttackCount)
            basicAttackIndex = 0;

        //重置速度
        rb.velocity = Vector3.zero;

        //每次攻击前可以调整一下转向
        player.SetFlip();

        //调整普攻的animator参数 设置速度
        animator.SetInteger("BasicAttackIndex",basicAttackIndex);
        rb.velocity = new Vector2(player.basicAttack_Velocity[basicAttackIndex].x*(player.isRight?1:-1),
            player.basicAttack_Velocity[basicAttackIndex].y);
    }
    
}
