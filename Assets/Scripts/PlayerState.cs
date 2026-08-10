using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//¼Ì³Ð»ùÀà×´Ì¬µÄPlayerState
public class PlayerState : BaseState
{
    protected Player player;
    public PlayerState(string stateName, StateMachine stateMachine, Entity entity) : base(stateName, stateMachine, entity)
    {
        player=(Player)entity;
    }
}
