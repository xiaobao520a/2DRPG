using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player_DieState : PlayerState
{
    public Player_DieState(string stateName, StateMachine stateMachine, Entity entity) : base(stateName, stateMachine, entity)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //½ûÓÃÊäÈë
        player.playerInputSet.Disable();

        //É¾³ıPlayer
        GameObject.Destroy(player.gameObject, 2f);
    }
}
