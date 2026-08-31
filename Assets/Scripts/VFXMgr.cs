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
    //收到伤害时的视觉特效材料
    [SerializeField] private Material onDamage_VFXMaterial;
    //伤害视觉持续时间
    [SerializeField] private float onDamage_VFXDurationTime=0.2f;


    //播放受伤时的视觉特效
    public void PlayDamageVFX(Entity entity)
    {
        SpriteRenderer sr=entity.GetComponentInChildren<SpriteRenderer>();
        Material originalMaterial = sr.material;

        StartCoroutine(PlayDamageVFX_Coroutine(sr,originalMaterial));
    }

    IEnumerator PlayDamageVFX_Coroutine(SpriteRenderer sr, Material originMaterial)
    {
        sr.material = onDamage_VFXMaterial;
        yield return new WaitForSeconds(onDamage_VFXDurationTime);
        sr.material=originMaterial;
    }
}
