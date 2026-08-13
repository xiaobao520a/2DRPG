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

        //如果dash在cd 那就等cd好之后 canDash再变为true
        if(!player.canDash)
        {
            player.dashTimer += Time.deltaTime;
            if(player.dashTimer>=player.dashCD)
            {
                player.canDash = true;
                player.dashTimer = 0f;
            }
        }

        //Player的所有状态都应该去检测是否dash 如果按下了dash键并且当前状态不是dash状态
        if(player.playerInputSet.Player.Dash.WasPressedThisFrame()&&stateMachine.CurrentState!=player.DashState&&player.canDash)
        {
            stateMachine.ChangeState(player.DashState);
            return;
        }

    }
}
