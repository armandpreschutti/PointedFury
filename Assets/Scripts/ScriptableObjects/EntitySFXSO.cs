using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SFX", menuName = "Entities/SFX")]
public class EntitySFXSO : ScriptableObject
{
    public List<AudioClip> _lightAttackWhooshSFX;
    public List<AudioClip> _heavyAttackWhooshSFX;
    public List<AudioClip> _lightAttackImpactSFX;
    public List<AudioClip> _heavyAttackImpactSFX;
    public List<AudioClip> _blockImpactSFX;
    public List<AudioClip> _parrySFX;
    public List<AudioClip> _dashSFX;
    public List<AudioClip> _deathSFX;
    public List<AudioClip> _deflectSFX;
    public List<AudioClip> _sprintSFX;
}
