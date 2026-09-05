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
                //有且只有一个Player 只有一个player的collider
                Collider2D hitCollider = null;

                //检测是否击中Player
                hitCollider=Physics2D.OverlapCircle(enemy.transform.position + new Vector3(enemy.attackOffset.x * (enemy.isRight ? 1 : -1), enemy.attackOffset.y,0),
                    enemy.attackRadius,enemy.playerLayer);

                bool isCrit;
                //如果击中了 就调用TakeDamage
                if (hitCollider != null)
                {
                    AttackHitData hitData = new AttackHitData()
                    {
                        //计算算上暴击之后的物理伤害
                        damage = enemy.entity_Attribute.GetPhysicalDamage(0, 0, 0, out isCrit),
                        isCrit = isCrit,
                        knockBackForce = enemy.knockBackForce,
                        knockBackDirection = enemy.player.transform.position.x - enemy.transform.position.x > 0 ? 1 : -1,
                        hitEntity = enemy,
                        armorPenetration = enemy.entity_Attribute.GetArmorPenetration()
                    };
                    hitCollider.GetComponent<IDamageable>().TakeDamage(hitData);
                }
                break;

                //攻击警报开始 也就是player可以反击的窗口开始
            case "Enemy_Skeleton_AttackAlertBegin":
                //此时敌人可以被击晕 反击窗口打开 打开!特效
                enemy.CanBeCountered = true;
                EventCenter.Instance.Broadcast<bool>(E_EventType.Enemy_AttackAlertBegin, true);
                    break;

            //攻击警报结束 也就是player可以反击的窗口开始
            case "Enemy_Skeleton_AttackAlertEnd":
                //此时敌人不可以被击晕 反击窗口关闭 关闭!特效
                enemy.CanBeCountered= false;
                EventCenter.Instance.Broadcast<bool>(E_EventType.Enemy_AttackAlertEnd, false);
                break;

            //如果结束的时候发现还能砍到player 那就继续砍
            case "Enemy_Skeleton_AttackEnd":
                    stateMachine.ChangeState(enemy.battleState);
                break;
        }
    }

    public override void Exit()
    {
        base.Exit();

        //退出的时候也要隐藏! 防止被弹反的时候还没到End窗口 同时关掉弹反窗口 以防万一
        EventCenter.Instance.Broadcast<bool>(E_EventType.Enemy_AttackAlertEnd, false);
        enemy.CanBeCountered = false;
    }
}
