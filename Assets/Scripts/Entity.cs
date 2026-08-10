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

    //初始化变量 组件
    protected virtual void Awake()
    {
        stateMachine = new StateMachine();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponentInChildren<Rigidbody2D>();
    }

    //一直执行状态机的Update
    protected virtual void Update()
    {
        stateMachine.Update();
    }
}
