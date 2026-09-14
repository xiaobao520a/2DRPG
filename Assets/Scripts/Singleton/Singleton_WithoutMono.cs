using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

/// <summary>
/// 不继承Monobehaviour的单例模式基类
/// </summary>
//抽象类 外部不能new这个基类去实例化
//暂时没有处理多线程的安全问题 因为现在项目还没用到多线程
public abstract class Singleton_WithoutMono<T> where T:class
{
    private static T instance;

    //懒加载
    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                //通过反射去调用私有的构造函数 私有是为了安全性考虑 防止有public的构造函数 外部可以调用去实例化
                Type type= typeof(T);
                ConstructorInfo info = type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, //表示成员私有方法
                    null,               //表示没有绑定对象
                    Type.EmptyTypes,    //表示没有参数
                    null);              //表示没有参数修饰符

                instance=info.Invoke(null) as T;
            }
               
            return instance;
        }
    }


}
