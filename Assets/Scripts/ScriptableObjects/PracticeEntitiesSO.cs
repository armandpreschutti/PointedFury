using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemies", menuName = "PointedFury/Practice/Enemies")]
public class PracticeEntitiesSO : ScriptableObject
{
    public GameObject[] WeakEnemies;
    public GameObject[] MediumEnmies;
    public GameObject[] HeavyEnmies;
    public GameObject[] BossEnemies;
}
