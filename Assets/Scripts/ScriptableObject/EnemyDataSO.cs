using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//所有Enemy共有的配置数据文件
public class EnemyDataSO : ScriptableObject
{
    [Header("移动相关")]
    public float idleTime = 2f; //Idle的时间
    public float moveSpeed = 5f; //移动速度

    [Header("血量相关")]
    public int nowHp; //当前hp
    public int maxHp; //最大hp

    [Header("战斗相关")]
    public float detectPlayer_Distance = 10f; //侦察玩家的距离
    public float maxBattleTime = 2f; //在没找到Player的情况下的battle最长维持时间
    public float battleSpeed = 6f; //battle状态的速度
    public float attackDistance = 2f; //攻击距离
    public float attackCD = 0.2f; //攻击的CD
    public float attackRadius = 1.5f; //攻击的半径
    public float attackAngle = 120f; //扇形的角度
    public Vector2 attackOffset = new Vector2(1f, 0); //攻击检测点的偏移量
    public Vector2 knockBackForce = new Vector2(3f, 2f); //击退力
    public float attackDamage = 10f; //普攻伤害

    [Header("击晕相关")]
    public float stunnedDuration; //击晕的时间
    public float stunnedVelocity; //被击晕后的水平速度

    [Header("属性相关")]
    public Attribute_AttackGroup attackGroup = new Attribute_AttackGroup();
    public Attribute_DefenseGroup defenseGroup= new Attribute_DefenseGroup();
    public float maxEvasion = 85; //闪避率上限



}
