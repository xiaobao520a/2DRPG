using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//所有Enemy共有的配置数据文件
public class EnemyDataSO : ScriptableObject
{
    public float idleTime = 2f; //Idle的时间
    public float moveSpeed = 5f; //移动速度
    public float detectPlayer_Distance=10f; //侦察敌人的距离
    public float maxBattleTime=2f; //在没找到Player的情况下的battle最长维持时间
    public float battleSpeed = 6f; //battle状态的速度
    public float attackDistance=2f; //攻击距离
    public float attackCD = 0.2f; //攻击的CD

}
