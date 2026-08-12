using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

//实体 Player Enemy等 的基类
public abstract class Entity:MonoBehaviour,IAnimationEventReceiver
{
    //都存在的组件
    protected StateMachine stateMachine;
    protected Animator animator;
    protected Rigidbody2D rb;

    //都需要的变量
    //物理 运动相关
    public float moveSpeed; //移动速度
    public bool isRight; //翻转相关

    //检测相关 地面检测 墙壁检测
    public bool isGround; //是否触地
    [SerializeField]private float groundDetect_Distance; //地面检测距中心点的距离
    [SerializeField] private LayerMask groundLayer; //地面层

    //墙壁检测 有上下两个检测点 更精准 Wall也用Ground地面层
    [SerializeField] private Transform topWallDetect_Transform;
    [SerializeField] private Transform bottomWallDetect_Transform;
    [SerializeField] private float wallDetect_Distance; //墙壁检测的距离
    public bool isWall; //是否检测到墙壁

    //初始化变量 组件
    protected virtual void Awake()
    {
        stateMachine = new StateMachine();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponentInChildren<Rigidbody2D>();
    }

    //一直执行状态机的Update 同时进行地面检测 墙壁检测
    protected virtual void Update()
    {
        stateMachine.Update();

        //地面检测
        DetectGround();

        //墙壁检测
        DetectWall();

    }

    protected virtual void Start()
    {
        //初始化一开始角色的朝向 是right还是left
        InitFlip();
    }

    //实现这个动画事件的接口 默认没有行为 子类需要就自行重写
    public virtual void OnAnimationEvent(string eventName)
    {

    }

    private void DetectGround()
    {
        if(Physics2D.Raycast(transform.position,Vector2.down,groundDetect_Distance,groundLayer))
            isGround = true;
        else
            isGround = false;
    }

    private void DetectWall()
    {
        //两层墙壁检测都成功的时候 isWall才是true
        if(Physics2D.Raycast(topWallDetect_Transform.position,isRight?Vector2.right:Vector2.left,wallDetect_Distance,groundLayer)
            &&Physics2D.Raycast(bottomWallDetect_Transform.position, isRight ? Vector2.right : Vector2.left, wallDetect_Distance, groundLayer))
            isWall = true;
        else
            isWall = false;
    }

    private void InitFlip()
    {
        if (transform.rotation == Quaternion.identity)
            isRight = true;
        else
            isRight = false;
    }

    //画出地面检测和墙壁检测的线 方便观察调试
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position+Vector3.down*groundDetect_Distance);
        Gizmos.DrawLine(topWallDetect_Transform.position, topWallDetect_Transform.position + (isRight ? Vector3.right : Vector3.left) * wallDetect_Distance);
        Gizmos.DrawLine(bottomWallDetect_Transform.position, bottomWallDetect_Transform.position + (isRight ? Vector3.right : Vector3.left) * wallDetect_Distance);

    }
}
