using UnityEngine;
using Cinemachine;
using JetBrains.Annotations;
using UnityEngine.Jobs;
using System;

public class PlayerCameraController : MonoBehaviour
{
    private StateMachine _stateMachine;

    // cinemachine
    private const float _threshold = 0.01f;
    public float _cinemachineTargetYaw;
    public float _cinemachineTargetPitch;
    private float _pitchResetTime;
    public float AttackCameraAimOffset;
    public float AttackCameraPositionScreenOffset;
    public float AttackCameraSideBuffer;

    [SerializeField] CinemachineVirtualCamera _freeRoamCamera;
    [SerializeField] CinemachineVirtualCamera _shortFightCamera;
    //[SerializeField] CinemachineVirtualCamera _zoomCamera;
    [SerializeField] CinemachineVirtualCamera _longFightCamera;
    [SerializeField] CinemachineVirtualCamera _sprintCamera;
    [SerializeField] CinemachineVirtualCamera _attackCamera;


    
    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject _cinemachineCameraTarget;

    [Tooltip("How far in degrees can you move the camera up and down while in combat")]
    public float FightTopClamp = 35f;
    public float FightBottomClamp = 35f;


    [Tooltip("How far in degrees can you move the camera up and down while in free roam")]
    public float FreeRoamTopClamp = 70f;
    public float FreeRoamBottomClamp = 70f;


    [Tooltip("For locking the camera position on all axis")]
    public bool LockCameraPosition = false;

    public float FightSensitivity;
    public float FreeRoamSensitivity;
    private float _bottomClamp = 0f;
    private float _topClamp = 0f;
    private float _yAxisSensitivity;
    private float _xAxisSensitivity;

    public float PitchResetDelay;
    public float FightPitchAngle;
    public float FreeRoamPitchAngle;
    public float PitchResetSpeed;

    private void Awake()
    {
        _stateMachine = GameObject.Find("Player").GetComponent<StateMachine>();
        _cinemachineCameraTarget = GameObject.Find("Player").transform.Find("PlayerCameraTarget").gameObject;
        _pitchResetTime = PitchResetDelay;
    }
    private void OnEnable()
    {
       
        EnemyManagementSystem.OnZoneEntered += SetCameraState;
        EnemyManagementSystem.OnLastEnemy += SetCameraState;
        EnemyManagementSystem.OnZoneEnemiesCleared += SetCameraState;
        PracticeEnemyManagementSystem.OnZoneEntered += SetCameraState;
        PracticeEnemyManagementSystem.OnLastEnemy += SetCameraState;
        PracticeEnemyManagementSystem.OnZoneEnemiesCleared += SetCameraState;
        //CutSceneTriggerHandler.onStartCutscene += ResetCameraPosition;
        _stateMachine.OnFight += SetLoneCameraState;
        _stateMachine.OnSprint += SetSprintCameraState;
        _stateMachine.OnLightAttack += SetAttackCameraState;
        _stateMachine.OnHeavyAttack += SetAttackCameraState;
        _stateMachine.OnParry += SetAttackCameraState;
    }
    private void OnDisable()
    {
        EnemyManagementSystem.OnZoneEntered -= SetCameraState;
        EnemyManagementSystem.OnLastEnemy -= SetCameraState;
        EnemyManagementSystem.OnZoneEnemiesCleared -= SetCameraState;
        PracticeEnemyManagementSystem.OnZoneEntered -= SetCameraState;
        PracticeEnemyManagementSystem.OnLastEnemy -= SetCameraState;
        PracticeEnemyManagementSystem.OnZoneEnemiesCleared -= SetCameraState;
        //CutSceneTriggerHandler.onStartCutscene -= ResetCameraPosition;
        _stateMachine.OnFight -= SetLoneCameraState;
        _stateMachine.OnSprint -= SetSprintCameraState;
        _stateMachine.OnLightAttack -= SetAttackCameraState;
        _stateMachine.OnHeavyAttack -= SetAttackCameraState;
        _stateMachine.OnParry -= SetAttackCameraState;
    }

    // Start is called before the first frame update    
    void Start()
    {
        ResetCameraPosition();
        _shortFightCamera.gameObject.SetActive(false);
        _longFightCamera.gameObject.SetActive(false);
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CameraRotation();
        SetCameraSensitity();
        CameraPitchPositioningLoop();
        if(_stateMachine.CurrentTarget != null)
        {
            Debug.Log(GetScreenSide(_stateMachine.gameObject, _stateMachine.CurrentTarget));
        }

    }

    public void ResetCameraPosition()
    {
        _cinemachineTargetYaw = _cinemachineCameraTarget.transform.rotation.eulerAngles.y;
    }


    public void SetCameraState(bool value, int enemies)
    {

        if (value)
        {

            if (enemies < 2)
            {
                _shortFightCamera.gameObject.SetActive(true);
                _longFightCamera.gameObject.SetActive(false);
            }
            else
            {
                _longFightCamera.gameObject.SetActive(true);
            }
        }
        else
        {
            _longFightCamera.gameObject.SetActive(false);
            _shortFightCamera.gameObject.SetActive(false);
        }
        //_longFightCamera.gameObject.SetActive(true);
    }

    public void SetLoneCameraState(bool value)
    {
        if(_stateMachine.EnemiesNearby != null && _stateMachine.EnemiesNearby.Length < 2)
        {
            _shortFightCamera.gameObject.SetActive(value);
        }

    }

    public void SetSprintCameraState(bool value)
    {
        _sprintCamera.gameObject.SetActive(value);
    }

    public void SetAttackCameraState(bool value, string attackType)
    {


        int enemies = _stateMachine.EnemiesNearby.Length;
        if (enemies == 1 && GetScreenSide(_stateMachine.gameObject, _stateMachine.CurrentTarget) != "N/A")
        {
            _attackCamera.m_LookAt = _stateMachine.CurrentTarget.transform.Find("PlayerCameraTarget");
            //Transform playerCameraTarget = _stateMachine.transform.Find("PlayerCameraTarget");
            if (GetScreenSide(_stateMachine.gameObject, _stateMachine.CurrentTarget) == "Right")
            {


                _attackCamera.GetCinemachineComponent<CinemachineComposer>().m_ScreenX = .5f - AttackCameraPositionScreenOffset;
                // _attackCamera.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset.x = AttackCameraAimOffset;
                // playerCameraTarget.rotation = Quaternion.Euler(playerCameraTarget.rotation.eulerAngles.x, -45, playerCameraTarget.rotation.eulerAngles.z);
            }
            else if (GetScreenSide(_stateMachine.gameObject, _stateMachine.CurrentTarget) == "Left")
            {
                _attackCamera.GetCinemachineComponent<CinemachineComposer>().m_ScreenX = .5f + AttackCameraPositionScreenOffset; ;
                // _attackCamera.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset.x = -AttackCameraAimOffset;
                //playerCameraTarget.rotation = Quaternion.Euler(playerCameraTarget.rotation.eulerAngles.x, 45, playerCameraTarget.rotation.eulerAngles.z);
            }
            else
            {
                _attackCamera.GetCinemachineComponent<CinemachineComposer>().m_ScreenX = .5f;
                //_attackCamera.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset.x = 0.0f;
            }
            _attackCamera.gameObject.SetActive(value);
        }
        else
        {
            _attackCamera.gameObject.SetActive(false);
        }

    }
    // Function to determine if the GameObject is on the left or right off the current target
    public string GetScreenSide(GameObject obj, GameObject otherObj)
    {
        float buffer = AttackCameraSideBuffer;
        // Get the world position of the player GameObject
        Vector3 playerPosition = obj.transform.position;
        // Get the world position of the enemy GameObject
        Vector3 enemyPosition = otherObj != null? otherObj.transform.position : Vector3.zero;

        // Convert the players world position to screen position
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(playerPosition);
        // Convert the enemys world position to screen position
        Vector3 enemyScreenPosition = Camera.main.WorldToScreenPoint(enemyPosition);
        // Get the width of the screen
        //float screenWidth = Screen.width;

        // Determine if the GameObject is on the left or right of the enemy
        if (playerScreenPosition.x < enemyScreenPosition.x - buffer)
        {
            return "Left";
        }
        else if(playerScreenPosition.x > enemyScreenPosition.x + buffer)
        {
            return "Right";
        }
        else
        {
            return "N/A";
        }
    }



    public void SetCameraSensitity()
    {
        _xAxisSensitivity = _stateMachine.IsFighting ? FightSensitivity : FreeRoamSensitivity;
        _yAxisSensitivity = _stateMachine.IsFighting ? FightSensitivity : FreeRoamSensitivity;
        _topClamp = _stateMachine.IsFighting ? FightTopClamp : FreeRoamTopClamp;
        _bottomClamp = _stateMachine.IsFighting ? FightBottomClamp : FreeRoamBottomClamp;
    }

    private void CameraRotation()
    {
        if (_stateMachine.LookInput.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            float deltaTimeMultiplier = Time.deltaTime;

            _cinemachineTargetYaw += (_stateMachine.LookInput.x * _xAxisSensitivity) * Time.deltaTime;
            _cinemachineTargetPitch += (_stateMachine.LookInput.y * _yAxisSensitivity) * Time.deltaTime;

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, _bottomClamp, _topClamp);
        }

        // Cinemachine will follow this target
        _cinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch, _cinemachineTargetYaw, 0.0f);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
    
    public void CameraPitchPositioningLoop()
    {

        if (_stateMachine.LookInput == Vector2.zero)
        {

            _pitchResetTime -= Time.deltaTime;

            // Check if the timer has reached zero
            if (_pitchResetTime <= 0)
            {
                float resetDuration = 0f;
                resetDuration += Time.deltaTime;
                if(_stateMachine.EnemiesNearby != null && _stateMachine.EnemiesNearby.Length > 1)
                {
                    _cinemachineTargetPitch = Mathf.Lerp(_cinemachineTargetPitch, FightPitchAngle, resetDuration * PitchResetSpeed);
                }
                else
                {
                    _cinemachineTargetPitch = Mathf.Lerp(_cinemachineTargetPitch, FreeRoamPitchAngle, resetDuration * PitchResetSpeed);
                }               
            }
        }
        else
        {
            _pitchResetTime = PitchResetDelay;
            return;
        }
    }
}
