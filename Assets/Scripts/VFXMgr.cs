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

}
