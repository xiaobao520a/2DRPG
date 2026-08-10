using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//实体 Player Enemy等 的基类
public abstract class Entity:MonoBehaviour
{
    //都存在的组件
    protected StateMachine stateMachine;
    protected Animator animator;
    protected Rigidbody2D rb;

    //都需要的变量
    //物理 运动相关
    public float moveSpeed; //移动速度
    public bool isRight; //翻转相关
    public bool isGround; //是否触地
    [SerializeField]private float groundDetect_Distance; //地面检测距中心点的距离
    [SerializeField] private LayerMask groundLayer; //地面层

    //初始化变量 组件
    protected virtual void Awake()
    {
        stateMachine = new StateMachine();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponentInChildren<Rigidbody2D>();
    }

    //一直执行状态机的Update 同时进行地面检测
    protected virtual void Update()
    {
        stateMachine.Update();

        DetectGround();
    }

    private void DetectGround()
    {
        if(Physics2D.Raycast(transform.position,Vector2.down,groundDetect_Distance,groundLayer))
            isGround = true;
        else
            isGround = false;
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position+Vector3.down*groundDetect_Distance);
    }
}
