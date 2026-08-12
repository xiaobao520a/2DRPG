using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//控制所有动画事件的桥
public class AnimationEventBridge : MonoBehaviour
{
    private IAnimationEventReceiver receiver;
    void Start()
    {
        receiver = GetComponentInParent<IAnimationEventReceiver>();
    }

   //所有的动画事件都在这 通过eventName区分 去需要的状态里面具体执行
    public void OnAnimationEvent(string eventName)
    {
        receiver.OnAnimationEvent(eventName);
    }

}
