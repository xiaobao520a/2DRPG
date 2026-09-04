using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class Player : Entity
{
    public PlayerInputSet playerInputSet; //输入配置文件
    [Header("配置文件")]
    public PlayerDataSO playerDataSO; //玩家相关变量的配置文件

    [Header("层级相关")]
    public LayerMask enemyLayer; //敌人的层级
    public LayerMask chestLayer; //箱子的层级

    [Header("移动/跳跃/滑动相关")]
    public Vector2 moveInput; //移动的输入
    public float jumpForce; //跳跃力
    public float inAir_Multiplier; //在空中 水平速度的乘积 让空中的水平速度不那么快
    public float wallSlideSpeed; //墙壁滑动状态的速度
    public float inWall_Multiplier; //墙壁滑动状态 竖直速度的乘积 不按s前的滑动速度会稍微慢一些
    public Vector2 wallJumpForce; //墙壁跳跃的力
    public float wallJumpTime; //墙壁跳跃状态持续的时间

    [Header("冲刺相关")]
    public float dashSpeed; //冲刺的速度
    public float dashTime; //冲刺能持续的时间
    public float dashCD; //冲刺冷却时间
    public bool canDash; //能否dash
    public float dashTimer; //计算是否冷却完成的计时器

    [Header("普攻相关")]
    public int basicAttackCount; //普攻有几段
    public float basicAttack_TimeWindow; //检测攻击键输入的最大时间 这段时间内输入就继续攻击 否则就退出攻击状态
    public List<Vector2> basicAttack_Velocity; //每段普攻进行的小幅度位移 的速度数组
    public float basicAttack_velocityTimeWindow; //每段攻击添加速度的时间 防止滑动太多 手感不好
    public float attackRadius;//攻击的半径
    public float attackAngle;//扇形的角度
    public Vector2 attackOffset; //攻击检测点的偏移量
    public Vector2 knockBackForce; //击退力
    public float attackDamage;
    public float knockBackDeceleration; //击退速度的衰减速率 防止被击退后一直滑动

    [Header("格挡/弹反相关")]
    public float parryDuration; //格挡状态持续的时间
    public float parryDetect_Radius; //格挡检测的半径
    public Vector2 parryDetect_Offset; //格挡检测点的偏移
    public float parryDetect_Angle; //格挡检测的角度
    public float counterDamage; //反击伤害
    public Vector2 counterKnockBackForce; //反击击退力
    public float counterDuration; //反击持续时间



    //Player的所有状态
    public Player_IdleState IdleState { get; private set; } //空闲状态
    public Player_MoveState MoveState { get; private set; } //移动状态
    public Player_JumpState JumpState { get; private set; } //跳跃状态
    public Player_FallState FallState { get; private set; } //下降状态
    public Player_WallSlideState WallSlideState { get; private set; } //墙壁滑动状态
    public Player_WallJumpState WallJumpState { get; private set; } //墙壁跳跃状态
    public Player_DashState DashState { get; private set; } //冲刺状态
    public Player_BasicAttackState BasicAttackState { get; private set; } //普攻状态
    public Player_DieState DieState { get; private set; } //死亡状态
    public Player_ParryState ParryState { get; private set; } //格挡状态
    public Player_CounterState CounterState { get; private set; } //反击状态



    protected override void Awake()
    {
        base.Awake();

        //初始化变量
        InitAllVariables();

        //初始化输入 开启输入监听
        InitInputSet();

        //初始化所有状态
        InitAllStates();
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

        //根据活力值 初始化最大Hp 和最开始的Hp
        maxHp=entity_Attribute.GetMaxHp();
        nowHp = maxHp;

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

    //初始化所有变量的方法
    private void InitAllVariables()
    {
        //初始化变量 从PlayerDataSO配置文件中去读取
        jumpForce = playerDataSO.jumpForce;
        moveSpeed = playerDataSO.moveSpeed;
        inAir_Multiplier = playerDataSO.inAir_Multiplier;
        wallSlideSpeed = playerDataSO.wallSlideSpeed;
        inWall_Multiplier = playerDataSO.inWall_multiplier;
        wallJumpForce = playerDataSO.wallJumpForce;
        dashSpeed = playerDataSO.dashSpeed;
        dashTime = playerDataSO.dashTime;
        basicAttackCount = playerDataSO.basicAttackCount;
        basicAttack_TimeWindow = playerDataSO.basicAttack_TimeWindow;
        basicAttack_Velocity = playerDataSO.basicAttack_Velocity;
        wallJumpTime = playerDataSO.wallJumpTime;
        dashCD = playerDataSO.dashCD;
        nowHp = playerDataSO.nowHp;
        maxHp = playerDataSO.maxHp;
        attackDamage = playerDataSO.attackDamage;
        basicAttack_velocityTimeWindow = playerDataSO.basicAttack_velocityTimeWindow;
        attackRadius = playerDataSO.attackRadius;
        attackAngle = playerDataSO.attackAngle;
        attackOffset = playerDataSO.attackOffset;
        knockBackForce = playerDataSO.knockBackForce;
        knockBackDeceleration = playerDataSO.knockBackDeceleration;
        parryDuration = playerDataSO.parryDuration;
        parryDetect_Radius = playerDataSO.parryDetect_Radius;
        parryDetect_Offset = playerDataSO.parryDetect_Offset;
        parryDetect_Angle = playerDataSO.parryDetect_Angle;
        counterDamage = playerDataSO.counterDamage;
        counterKnockBackForce = playerDataSO.counterKnockBackForce;
        counterDuration = playerDataSO.counterDuration;


        canDash = true;
        isDead = false;
    }

    //初始化输入的方法 开启输入监听
    private void InitInputSet()
    {
        //初始化输入
        playerInputSet = new PlayerInputSet();

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
    }

    //初始化所有状态
    private void InitAllStates()
    {
        //初始化所有的状态
        IdleState = new Player_IdleState("Idle", stateMachine, this);
        MoveState = new Player_MoveState("Move", stateMachine, this);
        JumpState = new Player_JumpState("JumpFall", stateMachine, this);
        FallState = new Player_FallState("JumpFall", stateMachine, this);
        WallSlideState = new Player_WallSlideState("WallSlide", stateMachine, this);
        WallJumpState = new Player_WallJumpState("WallJump", stateMachine, this);
        DashState = new Player_DashState("Dash", stateMachine, this);
        BasicAttackState = new Player_BasicAttackState("BasicAttack", stateMachine, this);
        DieState = new Player_DieState("Die", stateMachine, this);
        ParryState = new Player_ParryState("Parry", stateMachine, this);
        CounterState = new Player_CounterState("Counter", stateMachine, this);
    }

    public override void OnAnimationEvent(string eventName)
    {
        stateMachine.CurrentState?.OnAnimationEvent(eventName);
    }
    //水平翻转的函数
    public override void SetFlip()
    {
        //处理水平翻转
        if ((isRight && moveInput.x < 0) || (!isRight && moveInput.x > 0))
        {
            isRight = isRight ? false : true;
            transform.Rotate(Vector2.up, 180, Space.World);
        }

    }

    //去画出攻击的范围
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + (isRight ? 1 : -1) * (Vector3)attackOffset, attackRadius);

    }

    //Player死亡的方法
    public override void Die()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
        stateMachine.ChangeState(DieState);
    }

    public override void TakeDamage(AttackHitData hitData)
    {
        if (isDead) return;

        nowHp -= hitData.damage;
        //播放受伤特效
        EventCenter.Instance.Broadcast<Entity>(E_EventType.PlayerHurt, this);
        if (nowHp <= 0)
        {
            Die();
            return;
        }
        //击退方向由攻击方决定（knockBackDirection: 1=向右 -1=向左），只翻转X，保持Y不变
        rb.velocity = new Vector2(hitData.knockBackDirection * hitData.knockBackForce.x, hitData.knockBackForce.y);

        
        
    }
}
