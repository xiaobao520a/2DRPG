using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 继承Monobehaviour的自动挂载式的单例模式基类
/// </summary>
public abstract class Singleton_AutoWithMono<T> : MonoBehaviour where T:Singleton_AutoWithMono<T>
{
    private static T instance;
    public static T Instance
    {
        get
        {
            //如果初始化的时候 发现instance是null 就自动创建物体 挂载T脚本并且过场景不移除 全程自动处理
            if (instance == null)
            {
                GameObject obj = new GameObject();
                obj.name = typeof(T).Name;
                instance=obj.AddComponent<T>();
                DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }
    
}
