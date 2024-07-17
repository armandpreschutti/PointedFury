using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class PracticeManager : MonoBehaviour
{
    public PracticeEntitiesSO practiceEnemies;
    public GameObject player;
    public CinemachineBrain cinemachineBrain;
    public PlayableDirector playableDirector;
    public TimelineAsset introCutScene;
    public static Action OnEnableControl;
    public static Action OnBeginLevel;
    public Transform tempSpawn;
    public Collider spawnArea;
    public GameObject spawnVFX;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += LevelStarted;
        PracticeConfigController.OnSpawnEnemy += SpawnEnemy;

    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= LevelStarted;
        PracticeConfigController.OnSpawnEnemy -= SpawnEnemy;

    }

    public void LevelStarted(Scene currentScene, LoadSceneMode mode)
    {
        PlayIntroCutScene();
    }


    public void PlayIntroCutScene()
    {
        cinemachineBrain.m_DefaultBlend.m_Time = 1f;
        player.GetComponent<UserInput>().enabled = false;
        playableDirector.playableAsset = introCutScene;
        playableDirector.Play();
    }

    public void EnableControl()
    {
        OnEnableControl?.Invoke();
        player.GetComponent<UserInput>().enabled = true;
        player.GetComponent<CharacterController>().enabled = true;
        cinemachineBrain.m_DefaultBlend.m_Time = 1f;
    }
    public void BeginLevel()
    {
        OnBeginLevel?.Invoke();
    }

    public void SpawnEnemy()
    {
        Debug.Log("Practice Manager wants to spawn enemy");
        int randomEnemy = UnityEngine.Random.Range(0, practiceEnemies.WeakEnemies.Count);
        Vector3 randomPosition = GetRandomPosition();
        Instantiate(practiceEnemies.WeakEnemies[randomEnemy], randomPosition, Quaternion.identity);
        Instantiate(spawnVFX, randomPosition, Quaternion.identity);
    }

    

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("TitleMenu");
    }

    public Vector3 GetRandomPosition()
    {
        // Get the bounds of the collider
        Bounds bounds = spawnArea.bounds;

        // Generate random positions within the bounds
        float randomX = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
        float randomY = 0f;
        float randomZ = UnityEngine.Random.Range(bounds.min.z, bounds.max.z);

        // Return the random position
        return new Vector3(randomX, randomY, randomZ);
    }
}
