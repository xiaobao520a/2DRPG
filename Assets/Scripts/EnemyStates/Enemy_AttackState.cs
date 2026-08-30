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

                //如果击中了 就调用TakeDamage
                if (hitCollider != null)
                {
                    AttackHitData hitData = new AttackHitData()
                    {
                        damage = enemy.attackDamage,
                        knockBackForce = enemy.knockBackForce,
                        knockBackDirection = enemy.player.transform.position.x - enemy.transform.position.x > 0 ? 1 : -1,
                        hitEntity = enemy
                    };
                    hitCollider.GetComponent<IDamageable>().TakeDamage(hitData);
                }
                break;

                //如果结束的时候发现还能砍到player 那就继续砍
            case "Enemy_Skeleton_AttackEnd":
                    stateMachine.ChangeState(enemy.battleState);
                break;
        }
    }
}
