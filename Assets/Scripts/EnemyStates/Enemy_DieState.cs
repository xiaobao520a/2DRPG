using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DieState : EnemyState
{
    private Collider2D collider;
    public Enemy_DieState(string stateName, StateMachine stateMachine, Entity entity) : base(stateName, stateMachine, entity)
    {
        collider=enemy.GetComponent<Collider2D>();
    }

    //进入死亡应该重写Enter
    public override void Enter()
    {
        //禁用一系列组件 删除对象 
        animator.enabled = false;
        collider.enabled = false;
        enemy.enabled = false;

        //让对象快速的向上跳 再向下消失
        rb.gravityScale = 15;
        rb.velocity = new Vector2(0, 15);

        //2S后删除该对象
        GameObject.Destroy(enemy.gameObject, 2f);
    }

}
