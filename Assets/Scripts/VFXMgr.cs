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
        //PlayDamageVFX
        EventCenter.Instance.AddListener<HurtData>(E_EventType.PlayerHurt, PlayDamageVFX);
        EventCenter.Instance.AddListener<HurtData>(E_EventType.EnemyHurt, PlayDamageVFX);
        EventCenter.Instance.AddListener<Chest>(E_EventType.ChestOpen, PlayDamageVFX);

        //PlayEnemyAttackAlertVFX
        EventCenter.Instance.AddListener<bool>(E_EventType.Enemy_AttackAlertBegin, PlayEnemyAttackAlertVFX);
        EventCenter.Instance.AddListener<bool>(E_EventType.Enemy_AttackAlertEnd, PlayEnemyAttackAlertVFX);

        //PlayerHitVFX
        EventCenter.Instance.AddListener<HurtData>(E_EventType.PlayerHurt, PlayHitVFX);
        EventCenter.Instance.AddListener<HurtData>(E_EventType.EnemyHurt, PlayHitVFX);

        //PlayElementVFX
        EventCenter.Instance.AddListener<HurtData>(E_EventType.PlayerHurt, PlayElementVFX);
        EventCenter.Instance.AddListener<HurtData>(E_EventType.EnemyHurt, PlayElementVFX);

    }

    private void OnDisable()
    {
        //PlayDamageVFX
        EventCenter.Instance.RemoveListener<HurtData>(E_EventType.PlayerHurt, PlayDamageVFX);
        EventCenter.Instance.RemoveListener<HurtData>(E_EventType.EnemyHurt, PlayDamageVFX);
        EventCenter.Instance.RemoveListener<Chest>(E_EventType.ChestOpen, PlayDamageVFX);

        //PlayEnemyAttackAlertVFX
        EventCenter.Instance.RemoveListener<bool>(E_EventType.Enemy_AttackAlertBegin, PlayEnemyAttackAlertVFX);
        EventCenter.Instance.RemoveListener<bool>(E_EventType.Enemy_AttackAlertEnd, PlayEnemyAttackAlertVFX);

        //PlayerHitVFX
        EventCenter.Instance.RemoveListener<HurtData>(E_EventType.PlayerHurt, PlayHitVFX);
        EventCenter.Instance.RemoveListener<HurtData>(E_EventType.EnemyHurt, PlayHitVFX);

        //PlayElementVFX
        EventCenter.Instance.RemoveListener<HurtData>(E_EventType.PlayerHurt, PlayElementVFX);
        EventCenter.Instance.RemoveListener<HurtData>(E_EventType.EnemyHurt, PlayElementVFX);

        //删除所有协程 并清空记录
        StopAllCoroutines();
        elementVFXCoroutines.Clear();
    }

    [Header("受伤视觉特效相关")]
    //收到伤害时的视觉特效材料
    [SerializeField] private Material onDamage_VFXMaterial;
    //伤害视觉持续时间
    [SerializeField] private float onDamage_VFXDurationTime=0.2f;

    //正在闪烁的SpriteRenderer集合 每个渲染器独立闪烁 防止多目标同帧受击互相覆盖
    private HashSet<SpriteRenderer> flashingSet = new HashSet<SpriteRenderer>();

    [Header("敌人攻击预警相关")]
    [SerializeField] private GameObject enemy_AttackAlertObj;

    [Header("命中特效")]
    [SerializeField] private GameObject VFX_Hit;
    [SerializeField] private Color enemyHitColor= Color.yellow;
    [SerializeField] private Color playerHitColor = Color.gray;

    [Header("暴击特效")]
    [SerializeField] private GameObject VFX_CritHit;
    [SerializeField] private Color CritColor=Color.red;

    [Header("元素特效")]
    [SerializeField] private Color iceColor = Color.cyan;
    //正在播放元素特效的渲染器 -> 协程 每个渲染器独立 防止多目标互相覆盖
    private Dictionary<SpriteRenderer, Coroutine> elementVFXCoroutines = new Dictionary<SpriteRenderer, Coroutine>();


    //播放受伤时的视觉特效
    public void PlayDamageVFX(HurtData hurtData)
    {
        if (hurtData.hurtEntity == null || onDamage_VFXMaterial == null) return;

        SpriteRenderer sr = hurtData.hurtEntity.GetComponentInChildren<SpriteRenderer>();
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

    public void PlayEnemyAttackAlertVFX(bool isOpen)
    {
        enemy_AttackAlertObj.SetActive(isOpen);
    }

    //播放命中特效 也是在PlayerHurt和EnemyHurt事件中触发
    public void PlayHitVFX(HurtData hurtData)
    {
        //如果没有暴击 播放普通的Hit特效
        if (!hurtData.isCrit)
        {
            //播放特效 设置颜色 1s后删除特效
            GameObject obj = Instantiate(VFX_Hit, hurtData.hurtEntity.transform.position, Quaternion.identity);
            SpriteRenderer sr = obj.GetComponentInChildren<SpriteRenderer>();

            if (hurtData.hurtEntity is Player) sr.color = playerHitColor;
            else if (hurtData.hurtEntity is Enemy) sr.color = enemyHitColor;

            Destroy(obj, 1f);
        }

        //如果暴击了 播放暴击Hit特效
        else
        {
            GameObject obj = Instantiate(VFX_CritHit, hurtData.hurtEntity.transform.position, Quaternion.identity);
            SpriteRenderer sr = obj.GetComponentInChildren<SpriteRenderer>();
            sr.color = CritColor;

            Destroy(obj, 1f);
        }
    }

    //播放元素特效(ice fire lightning)
    public void PlayElementVFX(HurtData hurtData)
    {
        //元素类型为none 就不播
        if (hurtData.elementType == E_ElementType.none) return;
        if (hurtData.hurtEntity == null) return;

        SpriteRenderer sr = hurtData.hurtEntity.GetComponentInChildren<SpriteRenderer>();
        if (sr == null) return;

        //这个渲染器正在播元素特效 忽略这次
        if (elementVFXCoroutines.ContainsKey(sr)) return;

        //记录原始颜色 播完恢复 而不是写死白色
        Color originalColor = sr.color;
        elementVFXCoroutines[sr] = StartCoroutine(PlayElementVFX_Coroutine(sr, originalColor, hurtData.elementType,hurtData.elementDuration));
    }

    //播放元素特效的协程函数
    private IEnumerator PlayElementVFX_Coroutine(SpriteRenderer sr, Color originalColor, E_ElementType type,float duration)
    {
        try
        {
            switch (type)
            {
                case E_ElementType.ice:
                    Color lightColor = iceColor * 1.2f;
                    Color darkColor = iceColor * 0.8f;

                    //只切换一次颜色 从深色到浅色 持续时间是iceDuration的一半
                    for (int i = 0; i < 2; i++)
                    {
                        if (sr == null) yield break; //播放途中目标被销毁 直接结束
                        sr.color = (i == 0) ? lightColor : darkColor;
                        yield return new WaitForSeconds(duration / 2f);
                    }

                    //结束后恢复原来的颜色
                    if (sr != null)
                        sr.color = originalColor;
                    break;
            }
        }
        finally
        {
            //无论正常结束还是目标销毁 都移除记录 防止卡死后续元素特效
            elementVFXCoroutines.Remove(sr);
        }
    }

}
