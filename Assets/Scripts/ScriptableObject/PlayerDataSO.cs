using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//玩家数据的配置文件
[CreateAssetMenu(fileName = "PlayerDataSO", menuName = "SO/PlayerData")]
public class PlayerDataSO : ScriptableObject
{
    [Header("移动/跳跃参数")]
    public float jumpForce = 5f; //跳跃力
    public float inAir_Multiplier = 0.8f; //在空中 水平速度的乘积 让空中的水平速度不那么快
    public float moveSpeed = 5f; //移动速度

    [Header("墙壁相关")]
    public float inWall_multiplier = 0.7f;//墙壁滑动状态 竖直速度的乘积 不按s前的滑动速度会稍微慢一些
    public float wallSlideSpeed = 5f; //墙壁滑动状态的速度
    public Vector2 wallJumpForce = new Vector2(5f, 5f); //墙壁跳跃的力

    [Header("冲刺相关")]
    public float dashSpeed = 5f; //冲刺的速度
    public float dashTime = 0.5f; //冲刺能持续的时间

    [Header("普攻相关")]
    public int basicAttackCount = 3; //普攻有几段
    public float basicAttack_TimeWindow = 0.2f; //检测攻击键输入的最大时间 这段时间内输入就继续攻击 否则就退出攻击状态
    public List<Vector2> basicAttack_Velocity = new List<Vector2>() //每段普攻进行的小幅度位移 的速度数组
    {
        new Vector2(3,1.5f),new Vector2(1,2.5f),new Vector2(2.75f,1.75f)
    };







}
