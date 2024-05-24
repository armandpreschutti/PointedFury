using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPromptHandler : MonoBehaviour
{
    public EnemyManagementSystem enemyManagementSystem;
    public TutorialDetectionHandler tutorialDetectionHandler;
    public GameObject tutorialPrompt;

    private void OnEnable()
    {
        if(enemyManagementSystem != null)
        {
            enemyManagementSystem.OnInitiateTutorialUI += SetTutorialPromptState;
        }
        if(tutorialDetectionHandler!= null)
        {
            tutorialDetectionHandler.OnInitiateTutorialUI += SetTutorialPromptState;
        }

    }
    private void OnDisable()
    {
        if(enemyManagementSystem != null)
        {
            enemyManagementSystem.OnInitiateTutorialUI -= SetTutorialPromptState;
        }
        if (tutorialDetectionHandler != null)
        {
            tutorialDetectionHandler.OnInitiateTutorialUI -= SetTutorialPromptState;
        }

    }

    public void SetTutorialPromptState(bool value)
    {
        tutorialPrompt.SetActive(value);
    }
}
