using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemies", menuName = "PointedFury/Practice/Enemies")]
public class PracticeEntitiesSO : ScriptableObject
{
    public List<GameObject> WeakEnemies;
    public List<GameObject> MediumEnmies;
    public List<GameObject> HeavyEnmies;
    public List<GameObject> BossEnemies;
}
