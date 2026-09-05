using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//事件类型的枚举
public enum E_EventType 
{
   PlayerHurt, //玩家受伤 参数HurtData
   EnemyHurt, //敌人受伤 参数HurtData
   ChestOpen, //箱子打开 参数Chest
   Enemy_AttackAlertBegin, //敌人攻击警报开始 参数Bool
   Enemy_AttackAlertEnd, //敌人攻击警报关闭 参数Bool
}
