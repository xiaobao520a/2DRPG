using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// AB包管理器 管理资源的加载 卸载等
/// </summary>
public class ABMgr:MonoBehaviour
{
    private static ABMgr instance;
    public static ABMgr Instance=>instance;

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }


    //ab包字典 因为ab包只能加载一次 防止多次加载报错 记录已经加载过的ab包
    private Dictionary<string,AssetBundle>abDic= new Dictionary<string,AssetBundle>();

    //主包 用来加载包的依赖
    private AssetBundle abMain = null;

    //主包清单 用来加载包的依赖
    private AssetBundleManifest abMainManifest = null;

    /// <summary>
    /// AB包存放的路径 当前在Application.streamingAssetsPath下 可以修改
    /// </summary>
    private string pathUrl=Application.streamingAssetsPath;

    //不同平台下的默认主包名
    private string ABMainName
    {
        get
        {
#if UNITY_ANDROID
        return "Android";
#elif UNITY_IOS
        return "iOS";
#else
        return "StandaloneWindows";
#endif
        }

    }

    /// <summary>
    /// 加载ab包资源的方法 同步异步都在里面处理 永远异步加载包 而资源则选择是同步或异步
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <param name="callBack"></param>
    /// <param name="isAsync"></param>
    public void LoadRes(string abName,string resName,UnityAction<Object> callBack,bool isSync=false)
    {
        StartCoroutine(Co_LoadRes(abName,resName,callBack,isSync,null));
    }

    //2. 泛型版本
    public void LoadRes<T>(string abName, string resName, UnityAction<T> callBack, bool isSync=false) where T : Object
    {
        StartCoroutine(Co_LoadRes(abName, resName, obj => callBack(obj as T), isSync, typeof(T)));
    }


    //3. 指定类型版本
    public void LoadRes(string abName, string resName, System.Type type, UnityAction<Object> callBack, bool isSync=false)
    {
        StartCoroutine(Co_LoadRes(abName, resName, callBack, isSync, type));
    }
    //加载AB包的协程函数
    private IEnumerator Co_LoadRes(string abName, string resName, UnityAction<Object> callBack, bool isSync,System.Type assetType)
    {
        //先加载主包 主包清单 主包异步加载 主包的Manifest同步加载 因为这个manifest是所有包都要用的前置条件 必须先加载出来
        while ((abMain == null || abMainManifest == null))
        {
            if (!abDic.ContainsKey(ABMainName))
            {
                //占位
                abDic[ABMainName] = null;
                StartCoroutine(Co_LoadABMain());
            }

            else
                yield return null;
        }

        //加载当前包的所有依赖包
        string[] names = abMainManifest.GetAllDependencies(abName);
        foreach (string name in names)
        {
            if (!abDic.ContainsKey(name))
            {
                //占位
                abDic.Add(name, null);
                StartCoroutine(Co_LoadAB(name));
            }

            //一直等到包加载完毕 否则就一直等一帧检测一次加载完毕没有
            while(abDic[name] == null)
                    yield return null;
        }

        //加载当前包
        if (!abDic.ContainsKey(abName))
        {
            abDic[abName] = null;
            StartCoroutine(Co_LoadAB(abName));
        }

        //一直等到包加载完毕 否则就一直等一帧检测一次加载完毕没有
        while (abDic[abName] == null)
            yield return null;

        //根据isAsync来选择是同步加载资源 还是异步
        //同步加载资源
        if(isSync)
        {
            Object obj=assetType==null?abDic[abName].LoadAsset(resName): 
                abDic[abName].LoadAsset(resName, assetType);

            callBack(obj);  
        }

        //异步加载资源
        else
        {
            StartCoroutine(Co_ReallyLoadRes(abName, resName, callBack,assetType));
        }
    }

    
    //加载主包 主包Manifest 的异步协程函数
    private IEnumerator Co_LoadABMain()
    {
        var abcr=AssetBundle.LoadFromFileAsync(pathUrl+"/"+ABMainName);
        yield return abcr;

        if (abcr.assetBundle == null)
        {
            Debug.LogError("AB主包加载失败: " + pathUrl + "/" + ABMainName);
            abDic.Remove(ABMainName);          //移除占位 让后续调用能重试
            yield break;
        }

        abMain = abcr.assetBundle;
        abMainManifest = abMain.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        abDic[ABMainName] = abMain;
    }


    //加载AB包的 异步协程函数
    private IEnumerator Co_LoadAB(string abName)
    {
        var abcr=AssetBundle.LoadFromFileAsync(pathUrl+"/"+abName);
        yield return abcr;

        if (abcr.assetBundle == null)
        {
            Debug.LogError("AB包加载失败: " + pathUrl + "/" + abName);
            abDic.Remove(abName);          //移除占位 让后续调用能重试
            yield break;
        }

        abDic[abName]=abcr.assetBundle;
    }
   

    //真正的 拿到包之后 加载某一个资源的方法
    private IEnumerator Co_ReallyLoadRes(string abName,string resName,UnityAction<Object>callBack,System.Type assetType)
    {
        var abr=assetType==null?abDic[abName].LoadAssetAsync(resName):
            abDic[abName].LoadAssetAsync(resName, assetType);
        yield return abr;

        callBack(abr.asset);
    }


    //卸载的时候不需要异步 因为此时包已经在内存中 卸载很快 不需要硬盘IO 或者网络IO 而且可以放在加载条之类的地方卸载
    //同步卸载AB包 只卸载了目标包 没卸载对应的依赖包 false意味着只删包 true意味着把通过这个包加载出来的资源也全删了
    public void Unload(string abName,bool deleteRes=false)
    {
        if (abDic.ContainsKey(abName))
        {
            abDic[abName].Unload(deleteRes);
            abDic.Remove(abName);
        }
    }


    //同步卸载所有AB包 false意味着只删包 true意味着把通过这个包加载出来的资源也全删了
    public void UnLoadAll(bool deleteRes = false)
    {
        StopAllCoroutines();
        AssetBundle.UnloadAllAssetBundles(deleteRes);
        abDic.Clear();
    }
   

}
