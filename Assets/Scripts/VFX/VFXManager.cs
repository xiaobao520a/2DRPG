using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//特效管理器 单例 挂在一个空物体上
//监听EventCenter的PlayVFX事件 播放对应特效
public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance { get; private set; }

    [Header("特效配置")]
    [SerializeField] private List<VFXPrefabEntry> vfxPrefabList; //Inspector里配置 每种特效对应一个prefab

    private Dictionary<E_VFXType, GameObject> vfxPrefabDic; //运行时把List转成字典 方便查找

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        //特效要跨场景保留就打开这行
        //DontDestroyOnLoad(gameObject);

        vfxPrefabDic = new Dictionary<E_VFXType, GameObject>();
        foreach (VFXPrefabEntry entry in vfxPrefabList)
        {
            if (entry.prefab != null && !vfxPrefabDic.ContainsKey(entry.type))
                vfxPrefabDic.Add(entry.type, entry.prefab);
        }
    }

    private void OnEnable()
    {
        //EventCenter.Instance.AddListener(E_EventType.PlayVFX, OnPlayVFX);
    }

    private void OnDisable()
    {
        //ventCenter.Instance.RemoveListener(E_EventType.PlayVFX, OnPlayVFX);
    }

    //EventCenter回调 收到请求就播放
    private void OnPlayVFX(VFXData data)
    {
        if (!vfxPrefabDic.TryGetValue(data.type, out GameObject prefab) || prefab == null)
        {
            Debug.LogWarning($"VFXManager: 没有配置 {data.type} 的特效prefab");
            return;
        }

        //生成特效 放到指定位置
        GameObject vfx = Instantiate(prefab, data.position, Quaternion.identity);

        //方向朝左就翻转 默认prefab朝右 用缩放翻转比旋转稳
        if (data.direction.x < 0)
        {
            Vector3 scale = vfx.transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            vfx.transform.localScale = scale;
        }

        //有粒子系统就播一次 播完再销毁 避免特效被截断
        ParticleSystem ps = vfx.GetComponentInChildren<ParticleSystem>();
        if (ps != null)
        {
            ps.Play();
            float lifeTime = ps.main.duration + ps.main.startLifetime.constantMax;
            Destroy(vfx, lifeTime);
        }
        else
        {
            //动画类特效 没有粒子系统 先默认2秒销毁 可以改成动画总时长
            Destroy(vfx, 2f);
        }
    }
}

//特效播放请求 由受伤/攻击方构造并广播
public struct VFXData
{
    public E_VFXType type;    //播什么特效
    public Vector3 position;  //在哪播
    public Vector2 direction; //朝向 默认朝右
}

//Inspector配置项 一个类型对应一个prefab
[System.Serializable]
public class VFXPrefabEntry
{
    public E_VFXType type;
    public GameObject prefab;
}
