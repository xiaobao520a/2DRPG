using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//继承基类状态的PlayerState
public class PlayerState : BaseState
{
    protected Player player;
    public PlayerState(string stateName, StateMachine stateMachine, Entity entity) : base(stateName, stateMachine, entity)
    {
        player=(Player)entity;
    }

    public override void Update()
    {
        base.Update();

        //实时的改变YVelocity参数 控制Player Jump Fall这个动画BlendTree
        animator.SetFloat("YVelocity", rb.velocity.y);
    }
}
