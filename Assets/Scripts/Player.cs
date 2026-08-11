using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public PlayerInputSet playerInputSet; //输入配置文件
    public PlayerDataSO playerDataSO; //玩家相关变量的配置文件

    //移动 跳跃 运动 滑动等物理相关变量
    public Vector2 moveInput; //移动的输入
    public float jumpForce; //跳跃力
    public float inAir_Multiplier; //在空中 水平速度的乘积 让空中的水平速度不那么快
    public float wallSlideSpeed; //墙壁滑动状态的速度
    public float inWall_Multiplier; //墙壁滑动状态 竖直速度的乘积 不按s前的滑动速度会稍微慢一些
    public Vector2 wallJumpForce; //墙壁跳跃的力

    //Player的所有状态
    public Player_IdleState IdleState { get; private set; } //空闲状态
    public Player_MoveState MoveState { get; private set; } //移动状态
    public Player_JumpState JumpState { get; private set; } //跳跃状态
    public Player_FallState FallState { get; private set; } //下降状态
    public Player_WallSlideState WallSlideState { get; private set; } //墙壁滑动状态

    public Player_WallJumpState WallJumpState { get; private set; } //墙壁跳跃状态


    protected override void Awake()
    {
        base.Awake();

        //初始化输入
        playerInputSet = new PlayerInputSet();

        //初始化变量 从PlayerDataSO配置文件中去读取
        jumpForce = playerDataSO.jumpForce;
        moveSpeed = playerDataSO.moveSpeed; //水平速度
        inAir_Multiplier = playerDataSO.inAir_Multiplier;
        wallSlideSpeed = playerDataSO.wallSlideSpeed;
        inWall_Multiplier = playerDataSO.inWall_multiplier;
        wallJumpForce= playerDataSO.wallJumpForce;

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
        JumpState = new Player_JumpState("JumpFall", stateMachine, this);
        FallState = new Player_FallState("JumpFall", stateMachine, this);
        WallSlideState = new Player_WallSlideState("WallSlide", stateMachine, this);
        WallJumpState = new Player_WallJumpState("WallJump", stateMachine, this);
    }

    private void OnEnable()
    {
        //启用输入
        playerInputSet.Enable();
    }

    protected override void Start()
    {
        base.Start();
        //初始化状态机的初始状态
        stateMachine.Init(IdleState);
    }

    protected override void Update()
    {
        base.Update();

        
       
    }
    private void OnDisable()
    {
        //禁用输入
        playerInputSet.Disable();
    }

    //水平翻转的函数
    public void SetFlip()
    {
        //处理水平翻转
        if ((isRight && moveInput.x < 0) || (!isRight && moveInput.x > 0))
        {
            isRight = isRight ? false : true;
            transform.Rotate(Vector2.up, 180, Space.World);
        }

    }
}
