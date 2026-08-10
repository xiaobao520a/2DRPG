using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    private PlayerInputSet playerInputSet; //输入配置文件
    public Vector2 moveInput; //移动的输入


    //Player的所有状态
    public Player_IdleState IdleState { get; private set; } //空闲状态
    public Player_MoveState MoveState { get; private set; } //移动状态

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
        IdleState = new Player_IdleState("Idle",stateMachine,this);
        MoveState=new Player_MoveState("Move",stateMachine,this);
        moveSpeed = 5f; //水平速度
        isRight = true; //默认不翻转 也就是朝右

    }

    private void OnEnable()
    {
        //启用输入
        playerInputSet.Enable();
    }

    private void Start()
    {
        //初始化状态机的初始状态
        stateMachine.Init(IdleState);
    }

    private void OnDisable()
    {
        //禁用输入
        playerInputSet.Disable();
    }

    //水平翻转的函数
    public void Flip()
    {
        isRight=isRight?false:true;
        transform.Rotate(Vector2.up, 180,Space.World);
    }
}
