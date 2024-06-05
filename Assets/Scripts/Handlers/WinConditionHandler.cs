using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinConditionHandler : MonoBehaviour
{
    public HealthSystem bossHealthSystem;
    public static Action OnLevelPassed;
    public float winDelayTime = 5f;

    private void OnEnable()
    {
        bossHealthSystem.OnDeath += TriggerLevelComplete;
    }
    private void OnDisable()
    {
        bossHealthSystem.OnDeath -= TriggerLevelComplete;
    }

    public void TriggerLevelComplete()
    {
        StartCoroutine(LevelPassed());
    }

    public IEnumerator LevelPassed()
    {
        yield return new WaitForSeconds(winDelayTime);
        OnLevelPassed?.Invoke();
    }
}
