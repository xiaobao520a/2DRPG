using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_BasicAttackState : PlayerState
{
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
        basicAttackIndex = 0;
        animator.SetInteger("BasicAttackIndex", basicAttackIndex);

        //初始化各种变量
        isWindow = false;
        windowTime=player.basicAttack_TimeWindow;
        rb.velocity = new Vector2(player.basicAttack_Velocity[basicAttackIndex].x * (player.isRight ? 1 : -1),
            player.basicAttack_Velocity[basicAttackIndex].y * (player.isRight ? 1 : -1));
    }

    public override void Update()
    {
        base.Update();

        //锁死速度 让它就是配置好的攻击速度
        float lockedHorizontalSpeed = player.basicAttack_Velocity[basicAttackIndex].x
                                       * (player.isRight ? 1 : -1);
        rb.velocity = new Vector2(lockedHorizontalSpeed, rb.velocity.y);

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
                Debug.Log("普攻命中");
                break;
            case "BasicAttackEnd":
                isWindow = true;
                break;
        }
    }

    //执行下一次攻击
    private void PerformNextAttack()
    {
        isWindow = false;
        timer = 0f;
        basicAttackIndex++;
        //目前只有三段攻击
        if (basicAttackIndex >= player.basicAttackCount)
            basicAttackIndex = 0;

        //每次攻击前可以调整一下转向
        player.SetFlip();

        //调整普攻的animator参数 设置速度
        animator.SetInteger("BasicAttackIndex",basicAttackIndex);
        rb.velocity = new Vector2(player.basicAttack_Velocity[basicAttackIndex].x*(player.isRight?1:-1),
            player.basicAttack_Velocity[basicAttackIndex].y * (player.isRight ? 1 : -1));
    }
    
}
