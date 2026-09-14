using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 手动挂载式的继承Mono的单例模式基类
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class Singleton_WithMono<T> : MonoBehaviour where T : Singleton_WithMono<T>
{
    private static T instance;
    public static T Instance
    {
        get
        {
            return instance;
        }
    }

    //子类可以重写的Awake
    protected virtual void Awake()
    {
        //避免重复挂载 保证单例模式只有一个
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this as T;
        DontDestroyOnLoad(this.gameObject);
    }
}
