using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 技能UI节点
/// </summary> <summary>
/// 
/// </summary>
public class SkillNode : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    private bool isUnlocked=false; //是否解锁
    private bool isLocked=false; //是否锁定 锁定了就无法解锁

    [SerializeField]private Image img_Icon; //技能图标

    //技能锁住时候的颜色的十六进制
    [SerializeField]private string lockedHexColor="#BEBEBE";
    private Color unlockedColor=Color.white; //解锁时候的颜色

    public void OnPointerClick(PointerEventData eventData)
    {
        //如果未锁定且未解锁，则解锁技能 一直高亮颜色
        if(!isLocked&&!isUnlocked)
        {
            isUnlocked=true;
            img_Icon.color=unlockedColor;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //如果没有锁定 并且还未解锁 鼠标进入的时候高亮技能图标
        if(!isLocked&&!isUnlocked)
            img_Icon.color=unlockedColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //如果锁定 或者是未解锁 满足一个 那么退出的时候就恢复原来的暗色
        if(isLocked||!isUnlocked)
            img_Icon.color=ColorUtility.TryParseHtmlString(lockedHexColor,out Color color)?color:Color.gray;
    }

    //根据解锁状态 初始化当前节点是否高亮 
    private void Awake()
    {
        //如果是锁定状态 一直都是暗色
        if(isLocked) 
            img_Icon.color=ColorUtility.TryParseHtmlString(lockedHexColor,out Color color)?color:Color.gray;
        //如果是解锁状态 那就高亮
        if(isUnlocked)
            img_Icon.color=unlockedColor;
    }


}
