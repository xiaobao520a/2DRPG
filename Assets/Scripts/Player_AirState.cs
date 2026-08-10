using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//超状态 空中状态
public class Player_AirState : PlayerState
{
    public Player_AirState(string stateName, StateMachine stateMachine, Entity entity) : base(stateName, stateMachine, entity)
    {
    }

    public override void Update()
    {
        base.Update();

        //在空中的时候应该都能转向
        player.SetFlip();

        //设置空中的速度
        rb.velocity = new Vector2(player.moveSpeed*player.inAir_Multiplier*player.moveInput.x,rb.velocity.y);
    }
}
