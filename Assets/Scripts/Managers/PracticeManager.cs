using Cinemachine;
using System;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class PracticeManager : MonoBehaviour
{
    public PracticeEntitiesSO practiceEnemiesSO;
    public GameObject[] enemies;
    public GameObject player;
    public CinemachineBrain cinemachineBrain;
    public PlayableDirector playableDirector;
    public TimelineAsset introCutScene;
    public TimelineAsset GameOverCutScene;
    public static Action OnEnableControl;
    public static Action OnBeginLevel;
    public static Action OnEndLevel;
    public Transform tempSpawn;
    public Collider spawnArea;
    public GameObject spawnVFX;
    public bool isHealthActive = false;
    public int enemyTypeIndex = 0;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += LevelStarted;
        PracticeConfigController.OnSpawnEnemy += SpawnEnemy;
        PracticeConfigController.OnToggleHealthSystems += OnToggleHealthSystems;
        PracticeConfigController.OnCycleEnemyTypes += OnCycleEnemyTypes;

    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= LevelStarted;
        PracticeConfigController.OnSpawnEnemy -= SpawnEnemy;
        PracticeConfigController.OnToggleHealthSystems -= OnToggleHealthSystems;
        PracticeConfigController.OnCycleEnemyTypes -= OnCycleEnemyTypes;
    }

    private void Start()
    {
        enemies = practiceEnemiesSO.WeakEnemies;
    }

    public void LevelStarted(Scene currentScene, LoadSceneMode mode)
    {
        PlayIntroCutScene();
    }

    
    public void PlayerDeath(string value)
    {
        if(value == "Player")
        {
            Debug.Log("Player recognized as dead by practice manager");
            playableDirector.playableAsset = GameOverCutScene;
            playableDirector.Play();
        }

    }

    public void PlayIntroCutScene()
    {
        cinemachineBrain.m_DefaultBlend.m_Time = 1f;
        player.GetComponent<UserInput>().enabled = false;
        playableDirector.playableAsset = introCutScene;
        playableDirector.initialTime = 0;
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
        // Get a random enemy index and position
        int randomEnemyIndex = UnityEngine.Random.Range(0, enemies.Length);
        Vector3 randomPosition = GetRandomPosition();

        // Instantiate the enemy at the random position
        GameObject randomEnemy = Instantiate(enemies[randomEnemyIndex], randomPosition, Quaternion.identity);

        // Instantiate the spawn VFX at the random position
        Instantiate(spawnVFX, randomPosition, Quaternion.identity);

        // Get the HealthSystem component of the spawned enemy
        HealthSystem healthSystem = randomEnemy.GetComponent<HealthSystem>();

        // If the enemy has a HealthSystem component, set its active state based on isHealthActive
        if (healthSystem != null)
        {
            healthSystem.enabled = isHealthActive;
        }
    }

    public void OnToggleHealthSystems()
    {
        isHealthActive = !isHealthActive;

        // Find all HealthSystem components in the scene
        HealthSystem[] healthSystems = FindObjectsOfType<HealthSystem>();

        // Populate the array with the GameObjects
        for (int i = 0; i < healthSystems.Length; i++)
        {
            healthSystems[i].enabled = isHealthActive;
        }
        if(isHealthActive)
        {
            GameObject.Find("Player").GetComponent<HealthSystem>().EnableHealth();
        }
        else
        {
            GameObject.Find("Player").GetComponent<HealthSystem>().DisableHealth();

        }
    }
    
    public void OnCycleEnemyTypes()
    {
        switch (enemyTypeIndex)
        {
            case 0:
                enemies = practiceEnemiesSO.MediumEnmies;
                enemyTypeIndex = 1;
                break;
            case 1:
                enemies = practiceEnemiesSO.HeavyEnmies;
                enemyTypeIndex = 2;
                break;
            case 2:
                enemies = practiceEnemiesSO.BossEnemies;
                enemyTypeIndex = 3;
                break;
            case 3:
                enemies = practiceEnemiesSO.WeakEnemies;
                enemyTypeIndex = 0;
                break;
            default:
                enemies = practiceEnemiesSO.WeakEnemies;
                enemyTypeIndex = 1;
                break;
        }
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
