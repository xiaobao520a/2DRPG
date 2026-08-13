using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    //Enemy的通用状态
    public Enemy_IdleState idleState;
    public Enemy_MoveState moveState;
    public Enemy_AttackState attackState;
    public Enemy_BattleState battleState;

    //Enemy的通用变量
    public float idleTime; //idle状态持续的时间
    public float groundDetect_offsetX; //地面检测的点的X偏移量 因为要处理move到边缘转向的逻辑
    public float detectPlayer_Distance; //侦察Player的距离
    public LayerMask playerLayer; //Player的层级
    public RaycastHit2D playerHit; //通过射线检测 检测到的player
    [Range(0,2)]public float moveAnimSpeedMultiplier; //敌人移动速度的这个动画系数 快就大 慢就小 让动画速度贴近真实速度
    public float maxBattleTime; //在没找到Player的情况下的battle最长维持时间
    public float battleSpeed;
    public float attackDistance; //攻击距离
    public Player player;
    public float attackCD; //攻击的CD 攻击一次后等待一段时间才能再次攻击

    protected override void Start()
    {
        base.Start();

        stateMachine.Init(idleState);
    }

    //敌人侦察Player是否在附近的函数
    public bool DetectPlayer(ref RaycastHit2D playerHit)
    {
        playerHit=Physics2D.Raycast(transform.position, new Vector2(isRight ? 1 : -1, 0), detectPlayer_Distance, playerLayer);
        if (playerHit == default) return false;
        if (playerHit.transform.tag == "Player") return true;

        return false;
    }
    protected override void DetectGround()
    {
        if (Physics2D.Raycast(transform.position+new Vector3(isRight?groundDetect_offsetX:-groundDetect_offsetX,0,0), Vector2.down, groundDetect_Distance, groundLayer))
            isGround = true;
        else
            isGround = false;
    }

    //画出了 地面检测 墙壁检测 敌人检测的线 攻击范围 方便调试
    protected override void OnDrawGizmos()
    {
        Vector3 newWallDetectPosition = transform.position + new Vector3(isRight ? groundDetect_offsetX : -groundDetect_offsetX, 0, 0);
        Gizmos.DrawLine(newWallDetectPosition, newWallDetectPosition + Vector3.down * groundDetect_Distance);
        Gizmos.DrawLine(topWallDetect_Transform.position, topWallDetect_Transform.position + (isRight ? Vector3.right : Vector3.left) * wallDetect_Distance);
        Gizmos.DrawLine(bottomWallDetect_Transform.position, bottomWallDetect_Transform.position + (isRight ? Vector3.right : Vector3.left) * wallDetect_Distance);
        Gizmos.DrawLine(transform.position, transform.position + new Vector3((isRight ? 1 : -1) * detectPlayer_Distance,0,0));
        Gizmos.DrawLine(transform.position, transform.position + new Vector3((isRight ? 1 : -1) * attackDistance,0,0));
    }

    public override void OnAnimationEvent(string eventName)
    {
        stateMachine.CurrentState?.OnAnimationEvent(eventName);
    }
}
