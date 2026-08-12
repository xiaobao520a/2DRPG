using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 动画事件接收器接口
/// 任何需要动画事件的类都应该实现此接口
/// </summary>
public interface IAnimationEventReceiver
{
    void OnAnimationEvent(string eventName);
}
