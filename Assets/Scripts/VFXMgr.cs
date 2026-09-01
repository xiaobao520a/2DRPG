using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//视觉特效管理器 挂载Mono的单例模式
public class VFXMgr : MonoBehaviour
{
    private static VFXMgr instance;
    public static VFXMgr Instance=>instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    //添加EventCenter的监听
    private void OnEnable()
    {
        EventCenter.Instance.AddListener<Entity>(E_EventType.PlayerHurt, PlayDamageVFX);
        EventCenter.Instance.AddListener<Entity>(E_EventType.EnemyHurt, PlayDamageVFX);
        EventCenter.Instance.AddListener<Chest>(E_EventType.ChestOpen, PlayDamageVFX);
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveListener<Entity>(E_EventType.PlayerHurt, PlayDamageVFX);
        EventCenter.Instance.RemoveListener<Entity>(E_EventType.EnemyHurt, PlayDamageVFX);
        EventCenter.Instance.RemoveListener<Chest>(E_EventType.ChestOpen, PlayDamageVFX);
    }

    [Header("受伤视觉特效相关")]
    //收到伤害时的视觉特效材料
    [SerializeField] private Material onDamage_VFXMaterial;
    //伤害视觉持续时间
    [SerializeField] private float onDamage_VFXDurationTime=0.2f;

    //正在闪烁的SpriteRenderer集合 每个渲染器独立闪烁 防止多目标同帧受击互相覆盖
    private HashSet<SpriteRenderer> flashingSet = new HashSet<SpriteRenderer>();

    //播放受伤时的视觉特效
    public void PlayDamageVFX(Entity entity)
    {
        if (entity == null || onDamage_VFXMaterial == null) return;

        SpriteRenderer sr = entity.GetComponentInChildren<SpriteRenderer>();
        if (sr == null) return;

        //这个渲染器正在闪 忽略这次 防止抓到伤害材质导致卡死
        if (flashingSet.Contains(sr)) return;

        flashingSet.Add(sr);
        Material originalMaterial = sr.material;
        StartCoroutine(PlayDamageVFX_Coroutine(sr, originalMaterial));
    }

    //播放受伤时的视觉特效(箱子用) 重载
    public void PlayDamageVFX(Chest chest)
    {
        if (chest == null || onDamage_VFXMaterial == null) return;

        SpriteRenderer sr = chest.GetComponentInChildren<SpriteRenderer>();
        if (sr == null) return;

        //这个渲染器正在闪 忽略这次 防止抓到伤害材质导致卡死
        if (flashingSet.Contains(sr)) return;

        flashingSet.Add(sr);
        Material originalMaterial = sr.material;
        StartCoroutine(PlayDamageVFX_Coroutine(sr, originalMaterial));
    }
    IEnumerator PlayDamageVFX_Coroutine(SpriteRenderer sr, Material originalMaterial)
    {
        sr.material = onDamage_VFXMaterial;
        yield return new WaitForSeconds(onDamage_VFXDurationTime);

        flashingSet.Remove(sr);
        if (sr != null) //闪烁途中目标可能被销毁 别去摸它
            sr.material = originalMaterial;
    }
}
