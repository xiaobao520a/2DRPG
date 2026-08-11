using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//玩家数据的配置文件
[CreateAssetMenu(fileName ="PlayerDataSO",menuName ="SO/PlayerData")]
public class PlayerDataSO : ScriptableObject
{
    public float jumpForce=5f; //跳跃力
    public float inAir_Multiplier=0.8f; //在空中 水平速度的乘积 让空中的水平速度不那么快
    public float moveSpeed=5f; //移动速度
    public float inWall_multiplier = 0.7f;//墙壁滑动状态 竖直速度的乘积 不按s前的滑动速度会稍微慢一些
    public float wallSlideSpeed=5f; //墙壁滑动状态的速度
    public Vector2 wallJumpForce=new Vector2(5f,5f); //墙壁跳跃的力

}
