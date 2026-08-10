using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    private PlayerInputSet playerInputSet; //输入配置文件
    private Vector2 moveInput; //移动的输入


    //Player的所有状态
    private Player_IdleState idleState; //空闲状态

    protected override void Awake()
    {
        base.Awake();

        //初始化输入
        playerInputSet=new PlayerInputSet();

        //开启各种输入的监听 这里我加的是Lambda 所以如果频繁的失活激活其实会加很多监听函数
        //但是我的Player不会这样 所以我就暂时写在OnEnable了 也可以直接写在Awake或者Start就不存在这个问题
        //移动
        playerInputSet.Player.Move.performed += ((context) =>
        {
            moveInput = context.ReadValue<Vector2>();
        });

        playerInputSet.Player.Move.canceled += ((context) =>
        {
            moveInput = Vector2.zero;
        });

        //初始化所有的状态
        idleState = new Player_IdleState("Player_Idle",stateMachine,this);
    }

    private void OnEnable()
    {
        //启用输入
        playerInputSet.Enable();
    }

    private void Start()
    {
        //初始化状态机的初始状态
        stateMachine.Init(idleState);
    }

    private void OnDisable()
    {
        //禁用输入
        playerInputSet.Disable();
    }

}
