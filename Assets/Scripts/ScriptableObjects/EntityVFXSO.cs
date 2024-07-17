using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VFX", menuName = "PointedFury/Entities/VFX")]
public class EntityVFXSO : ScriptableObject
{
    public float LightAttackStartLifeTime;
    public float LightAttackStartSize;
    public bool LightAttackCOL;
    public float HeavyAttackStartLifeTime;
    public float HeavyAttackStartSize;
    public bool HeavyAttackCOL;
}

