using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//事件中心 用于解耦各个模块的复杂耦合
public class EventCenter
{
    private static EventCenter instance;
    public static EventCenter Instance
    {
        get
        {
            if(instance == null)
                instance = new EventCenter();
            return instance;
        }
    }

    private Dictionary<E_EventType, Delegate> eventDic = new Dictionary<E_EventType, Delegate>(); //管理事件的字典 里氏替换原则

    //无参广播 通知订阅对应事件的 所有委托(函数)执行
    public void Broadcast(E_EventType type)
    {
        //如果有这个事件类型
        if (eventDic.ContainsKey(type))
            (eventDic[type] as Action).Invoke();
    }

    //有参广播
    public void Broadcast<T>(E_EventType type,T data)
    {
        if(eventDic.ContainsKey(type))
            (eventDic[type] as Action<T>).Invoke(data);
    }

    //无参 为对应的事件添加订阅的方法
    public void AddListener(E_EventType type,Action listener)
    {
        //相当于+= 只是由于这个Delegate基类没有+= +=的底层就是Combine
        if (eventDic.ContainsKey(type))
            eventDic[type] = Delegate.Combine(eventDic[type], listener);
        else
            eventDic.Add(type, listener);
    }

    //

    //无参 对应事件退订的方法
    public void RemoveListener(E_EventType type,Action listener)
    {
        if(eventDic.ContainsKey(type))
        {
            //同理 相当于-=
            eventDic[type]= Delegate.Remove(eventDic[type],listener);

            //如果删除后 这个事件没有任何订阅了 就直接删除
            if (eventDic[type]==null)
                eventDic.Remove(type);
        }
    }



}
