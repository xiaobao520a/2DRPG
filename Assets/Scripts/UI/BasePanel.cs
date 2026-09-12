using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UI面板基类
/// </summary>
public abstract class BasePanel : MonoBehaviour
{
    //UIBehaviour是所有UGUI控件的父类
    /// <summary>
    /// 用于存储所有要用到的UI控件
    /// </summary>
    protected Dictionary<string,UIBehaviour>controlDic=new Dictionary<string,UIBehaviour>();

    /// <summary>
    /// 拥有Image组件之类的 控件的默认名字 如果得到的名字跟默认名字一样 那么它就只是起到显示作用的控件 不需要得到它来使用
    /// </summary>
    private static List<string> defaultNameList = new List<string>() { "Image","Text (TMP)","RawImage","Background",
    "Checkmark","Label","Text (Legacy)","Arrow","Placeholder","Fill","Handle","Viewport","Scrollbar Horizontal",
    "Scrollbar Vertical"};
    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        //UGUI的常用全部组件
        //为了避免某一个对象上存在两种或多种控件的情况下
        //我们应该优先查找重要组件 得到这些重要组件之后 下面的text image之类的都能找到
        FindChildrenControls<Button>();
        FindChildrenControls<Toggle>();
        FindChildrenControls<Slider>();
        FindChildrenControls<InputField>();
        FindChildrenControls<ScrollRect>();
        FindChildrenControls<Dropdown>();

        //即使对象上挂载了多个组件 只要优先找到了重要组件
        //之后也可以通过重要组件得到身上其他挂载的内容
        FindChildrenControls<Text>();
        FindChildrenControls<TextMeshProUGUI>();
        FindChildrenControls<Image>();
    }
    protected virtual void ClickBtn(string btnName)
    {

    }

    protected virtual void SliderValueChanged(string sliderName,float value)
    {

    }

    protected virtual void ToggleValueChanged(string toggleName, bool value)
    {

    }

    //找到面板下子物体的所有UI控件
    private void FindChildrenControls<T>() where T : UIBehaviour
    {
        //true意味着失活的也能得到
        T[] controls = GetComponentsInChildren<T>(true);
        foreach (T control in controls)
        {
            string controlName=control.gameObject.name;

            if (!controlDic.ContainsKey(control.gameObject.name))
            {
                if (!defaultNameList.Contains(control.gameObject.name))
                {
                    controlDic.Add(control.gameObject.name, control);

                    //判断控件的类型 决定是否加事件监听
                    if (control is Button)
                    {
                        (control as Button).onClick.AddListener(() =>
                        {
                            ClickBtn(controlName);
                        });
                    }

                    else if (control is Slider)
                    {
                        (control as Slider).onValueChanged.AddListener((value) =>
                        {
                            SliderValueChanged(controlName, value);
                        });
                    }

                    else if (control is Toggle)
                    {
                        (control as Toggle).onValueChanged.AddListener((value) =>
                        {
                            ToggleValueChanged(controlName, value);
                        });
                    }
                }
            }
        }
    }

    /// <summary>
    /// 显示面板
    /// </summary>
    public virtual void Show()
    {
    }

    /// <summary>
    /// 隐藏面板
    /// </summary>
    public virtual void Hide()
    {
    }

    /// <summary>
    /// 得到具体控件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    //name肯定是知道的 看UI面板都看得出来
    public T GetControl<T>(string name) where T : UIBehaviour
    {
        if (controlDic.ContainsKey(name))
        {
            if (!controlDic[name] is T)
            {
                Debug.LogWarning($"无法得到{name}类型为{typeof(T)}的组件");
                return null;
            }

            T control = controlDic[name] as T;
            return control;
        }
        else
        {
            Debug.LogWarning($"不存在对应名字{name}类型为{typeof(T)}的组件");
            return null;
        }

    }

}
