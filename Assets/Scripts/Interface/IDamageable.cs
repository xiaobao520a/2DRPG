using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//可被伤害 破坏的接口 Entity Chest这种都继承
public interface IDamageable
{
    //承受伤害
    public void TakeDamage(AttackHitData hitData);
}
