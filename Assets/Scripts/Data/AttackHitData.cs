using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//攻击命中Data的数据集合 值类型
public struct AttackHitData
{
    public float damage; //伤害 箱子之类的可以忽略
    public Vector2 knockBackForce; //击退力
    public float knockBackDirection; //1朝右击退 -1朝左击退
    public Entity hitEntity; //攻击者
}
