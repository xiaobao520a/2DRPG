using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//玩家数据的配置文件
[CreateAssetMenu(fileName = "PlayerDataSO", menuName = "SO/PlayerData")]
public class PlayerDataSO : ScriptableObject
{
    [Header("移动/跳跃参数")]
    public float jumpForce = 15f; //跳跃力
    public float inAir_Multiplier = 0.8f; //在空中 水平速度的乘积 让空中的水平速度不那么快
    public float moveSpeed = 8f; //移动速度

    [Header("墙壁相关")]
    public float inWall_multiplier = 0.4f; //墙壁滑动状态 竖直速度的乘积 不按s前的滑动速度会稍微慢一些
    public float wallSlideSpeed = 3f; //墙壁滑动状态的速度
    public Vector2 wallJumpForce = new Vector2(10f, 13f); //墙壁跳跃的力
    public float wallJumpTime = 0.3f; //墙壁跳跃状态持续的时间

    [Header("冲刺相关")]
    public float dashSpeed = 20f; //冲刺的速度
    public float dashTime = 0.25f; //冲刺能持续的时间
    public float dashCD = 0.3f; //冲刺的冷却时间

    [Header("战斗相关")]
    public float nowHp = 100f; //当前hp
    public float maxHp = 100f; //最大hp

    public int basicAttackCount = 3; //普攻有几段
    public float basicAttack_TimeWindow = 0.2f; //检测攻击键输入的最大时间 这段时间内输入就继续攻击 否则就退出攻击状态
    public List<Vector2> basicAttack_Velocity = new List<Vector2>() //每段普攻进行的小幅度位移 的速度数组
    {
        new Vector2(3,1.5f),new Vector2(1,2.5f),new Vector2(5,2.5f)
    };
    public float basicAttack_velocityTimeWindow = 0.3f; //每段攻击添加速度的时间 防止滑动太多 手感不好
    public float attackDamage = 10f; //基础普攻伤害
    public float attackRadius = 1.2f; //攻击的半径
    public float attackAngle = 120f; //扇形的角度
    public Vector2 attackOffset = new Vector2(0.5f, 0f); //攻击检测点的偏移量
    public Vector2 knockBackForce = new Vector2(3f, 2f); //击退力
    public float knockBackDeceleration = 12f; //击退速度的衰减速率 防止被击退后一直滑动

    [Header("格挡/弹反相关")]
    public float parryDuration = 0.5f; //格挡状态持续的时间
    public float parryDetect_Radius = 1.2f; //格挡检测的半径
    public Vector2 parryDetect_Offset = new Vector2(0.5f, 0f); //格挡检测点的偏移
    public float parryDetect_Angle = 120f; //格挡检测的角度
    public float counterDamage = 20f; //反击伤害
    public Vector2 counterKnockBackForce = new Vector2(5f, 4f); //反击击退力
    public float counterDuration = 0.2f; //反击持续时间

    [Header("属性/成长相关")]
    public float vitalityToHp=1f; //每点活力增加的最大HP

    public float agilityToEvasion=0.5f; //每点敏捷增加的闪避率
    public float agilityToCritChance=0.3f; //每点敏捷提供的暴击率

    public float strengthToDamage=1f; //每点力量提供的物理伤害
    public float strengthToCritPower=0.5f; //每点力量提供的暴击力量

    public float maxEvasion=85; //闪避率上限

    [Header("属性组")]
    public Attribute_MajorGroup majorGroup = new Attribute_MajorGroup(); //力量 敏捷 智力 活力
    public Attribute_AttackGroup attackGroup = new Attribute_AttackGroup(); //物理/暴击/元素攻击
    public Attribute_DefenseGroup defenseGroup = new Attribute_DefenseGroup(); //护甲 闪避 元素抗性
}
