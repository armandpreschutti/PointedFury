using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneTriggerHandler : MonoBehaviour
{
    public static Action onStartCutscene;

    private void OnDestroy()
    {
        onStartCutscene?.Invoke();
    }
}
