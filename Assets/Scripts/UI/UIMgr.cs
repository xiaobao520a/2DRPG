using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// UI层级 四层
/// </summary>
public enum E_UILayer
{
    Bottom,
    Middle,
    Top,
    System
}

/// <summary>
/// UI管理器 提供显示 隐藏 得到UI面板等方法
/// </summary>
public class UIMgr
{
    //抽象的PanelInfo的基类 用于存储 里氏替换原则
    private abstract class BasePanelInfo { }

    //PanelDic中存储的值
    private class PanelInfo<T> :BasePanelInfo where T:BasePanel
    {
        public T panel; //具体的panel
        public UnityAction<T> action; //回调函数

        public PanelInfo(UnityAction<T> callBack)
        {
            action += callBack;
        }

    }

    private static UIMgr instance;
    public static UIMgr Instance
    {
        get
        {
            if (instance == null)
                instance = new UIMgr();
            return instance;
        }
    }
    private UIMgr()
    {
        //初始化时动态创建Canvas和EventSystem(唯一 过场景不移除)
        canvas = GameObject.Instantiate(Resources.Load<GameObject>("UI/Canvas")).GetComponent<Canvas>();
        GameObject.DontDestroyOnLoad(canvas.gameObject);

        eventSystem = GameObject.Instantiate(Resources.Load<GameObject>("UI/EventSystem")).GetComponent<EventSystem>();
        GameObject.DontDestroyOnLoad(eventSystem.gameObject);

        //如果渲染模式是ScreenSpaceCamera 就需要一个UICamera
        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            UICamera = GameObject.Instantiate(Resources.Load<GameObject>("UI/UICamera")).GetComponent<Camera>();
            canvas.worldCamera = UICamera;
            GameObject.DontDestroyOnLoad(UICamera.gameObject);
        }

        //得到层级
        bottomLayer = canvas.transform.Find("Bottom").transform;
        middleLayer = canvas.transform.Find("Middle").transform;
        topLayer = canvas.transform.Find("Top").transform;
        systemLayer = canvas.transform.Find("System").transform;

    }

    //这些通用的UI相关的组件 因为很小 所以为了方便默认就直接用Resources去加载
    private Canvas canvas;
    private EventSystem eventSystem;
    private Camera UICamera; //渲染模式是ScreenSpaceCamera就需要

    //UI层级相关
    private Transform bottomLayer;
    private Transform middleLayer;
    private Transform topLayer;
    private Transform systemLayer;

    /// <summary>
    /// 管理所有面板的字典
    /// </summary>
    private Dictionary<string, BasePanelInfo> panelDic = new Dictionary<string, BasePanelInfo>();

    /// <summary>
    /// 显示面板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="layer">层级</param>
    /// <param name="callBack"></param>
    //这里用回调函数返回面板是因为 加载资源时ab包可能是异步加载 没法直接返回
    public void ShowPanel<T>(E_UILayer layer,UnityAction<T>callBack=null,bool isSync=false) where T : BasePanel
    {
        string name=typeof(T).Name;

        //如果字典里有键
        if (panelDic.ContainsKey(name))
        {
            PanelInfo<T> panelInfo = panelDic[name] as PanelInfo<T>;

            //null说明 正在异步加载panel 把回调函数+进panelInfo中 等待加载完毕后统一调用
            if (panelInfo.panel == null)
                panelInfo.action += callBack;

            //失活状态就直接激活就行
            else if(!panelInfo.panel.gameObject.activeSelf)
            {
                panelInfo.panel.gameObject.SetActive(true);
                panelInfo.panel.Show();
                callBack?.Invoke(panelInfo.panel);
            }

            //不是null说明已经有该面板 直接用就行 调用回调函数
            else
                callBack?.Invoke(panelInfo.panel);
        }

        //如果字典没键 加载面板
        else
        {
            //先加键 占位 说明这个面板正在加载 如果同一帧还要show这个panel 就把callBack加进去 防止出错
            panelDic.Add(name, new PanelInfo<T>(callBack));

            ABMgr.Instance.LoadRes<GameObject>("ui", name, (panelObj) =>
            {
                //如果发现键没了 说明需要hidePanel而且已经删掉键了 直接return
                if (!panelDic.TryGetValue(name, out BasePanelInfo info)) return;

                PanelInfo<T> panelInfo = info as PanelInfo<T>;

                //加载完成后先创建面板 然后再调用里面的回调函数 调用面板的show
                GameObject obj = GameObject.Instantiate(panelObj, GetFatherLayer(layer), false);
                T panel = obj.GetComponent<T>();

                panelInfo.panel = panel;
                panel.Show();
                panelInfo.action?.Invoke(panel);

                //调用完成后应该删除引用 防止内存泄露
                panelInfo.action = null;

            }, isSync);
        }
    }

    /// <summary>
    /// 隐藏面板 可选删除或者失活面板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void HidePanel<T>(bool isDestroy=true) where T : BasePanel
    {
        string name=typeof(T).Name;

        //如果有键
        if (panelDic.ContainsKey(name))
        {
            PanelInfo<T> panelInfo=panelDic[name] as PanelInfo<T>;
            
            //如果当前面板是null 说明此时正在加载面板
            if (panelInfo.panel == null)
            {
                //说明需要隐藏并且 删掉里面的回调函数 删除键 不再执行了具体隐藏的逻辑去异步加载那
                panelInfo.action = null;
                panelDic.Remove(name);
            }

            //如果当前面板不是null 说明已经存在该面板 可以直接删或失活
            else
            {
                panelInfo.panel.Hide();

                //删除面板
                if (isDestroy)
                {
                    GameObject.Destroy(panelInfo.panel.gameObject);
                    panelDic.Remove(name);
                }
                //失活面板
                else
                {
                    panelInfo.panel.gameObject.SetActive(false);
                    panelInfo.action = null;
                }
            }
            
        }

        //没键说明没有这个面板 直接不用管
    }

    /// <summary>
    /// 得到面板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public void GetPanel<T>(UnityAction<T>callBack) where T : BasePanel
    {
        string name= typeof(T).Name;

        //如果有键
        if(panelDic.ContainsKey(name))
        {
            PanelInfo<T> panelInfo = panelDic[name] as PanelInfo<T>;

            //如果panel是null 说明正在加载面板 把回调函数加进action 加载完后用就行
            if (panelInfo.panel == null)
                panelInfo.action += callBack;

            //如果panel不是null 
            else
            {
                //如果处于激活状态 直接拿到panel 调用回调函数
                if(panelInfo.panel.gameObject.activeSelf)
                    callBack?.Invoke(panelInfo.panel);

                //如果处于失活状态 不能用 不处理
                else
                    Debug.LogWarning($"{name}面板处于失活状态");
            }
        }

        //没键就说明都没有面板 弹警报
        else
            Debug.LogWarning($"不存在{name}面板");
    }

    /// <summary>
    /// 添加自定义监听
    /// </summary>
    /// <param name="control">具体控件</param>
    /// <param name="type">事件类型</param>
    /// <param name="callBack">回调函数</param>
    public void AddCustomEventListener(UIBehaviour control,EventTriggerType type,UnityAction<BaseEventData>callBack)
    {
        //得到EventTrigger组件
        EventTrigger eventTrigger=control.gameObject.GetComponent<EventTrigger>();
        if(eventTrigger == null)
            eventTrigger=control.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry=new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(callBack);

        eventTrigger.triggers.Add(entry);
    }

    //得到父级层级
    private Transform GetFatherLayer(E_UILayer layer)
    {
        switch(layer)
        {
            case E_UILayer.Bottom:
                return bottomLayer;

            case E_UILayer.Middle:
                return middleLayer;

            case E_UILayer.Top:
                return topLayer;

            case E_UILayer.System:
                return systemLayer;

            //默认在中间层
            default:
                return middleLayer;
        }
    }
}
