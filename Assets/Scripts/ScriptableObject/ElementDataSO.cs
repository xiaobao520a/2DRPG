using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//元素数据
[CreateAssetMenu(fileName =("ElementDataSO"),menuName = ("SO/ElementDataSO"))]
public class ElementDataSO : ScriptableObject
{
    [Header("冰元素")]
    public float iceDuration; //持续时间
    public float slowDownMoveSpeed_Multiplier; //减慢移动速度的乘数
    public float slowDownAnimationSpeed_Multiplier; //减慢动画速度的乘数
}
