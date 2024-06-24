using UnityEngine;
using Cinemachine;
using JetBrains.Annotations;
using UnityEngine.Jobs;

public class PlayerCameraController : MonoBehaviour
{
    private StateMachine _stateMachine;

    // cinemachine
    private const float _threshold = 0.01f;
    public float _cinemachineTargetYaw;
    public float _cinemachineTargetPitch;
    private float _pitchResetTime;

    [SerializeField] CinemachineVirtualCamera _freeRoamCamera;
    [SerializeField] CinemachineVirtualCamera _shortFightCamera;
    [SerializeField] CinemachineVirtualCamera _zoomCamera;
    [SerializeField] CinemachineVirtualCamera _longFightCamera;
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
        //CutSceneTriggerHandler.onStartCutscene += ResetCameraPosition;
        _stateMachine.OnFight += SetLoneCameraState;
       // _stateMachine.OnLightAttack += SetAttackCameraState;
        //_stateMachine.OnHeavyAttack += SetAttackCameraState;
    }
    private void OnDisable()
    {
        EnemyManagementSystem.OnZoneEntered -= SetCameraState;
        EnemyManagementSystem.OnLastEnemy -= SetCameraState;
        EnemyManagementSystem.OnZoneEnemiesCleared -= SetCameraState;
        //CutSceneTriggerHandler.onStartCutscene -= ResetCameraPosition;
         _stateMachine.OnFight -= SetLoneCameraState;
       // _stateMachine.OnLightAttack -= SetAttackCameraState;
       // _stateMachine.OnHeavyAttack -= SetAttackCameraState;
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

    }

    public void ResetCameraPosition()
    {
        _cinemachineTargetYaw = _cinemachineCameraTarget.transform.rotation.eulerAngles.y;
    }


    public void SetCameraState(bool value, int enemies)
    {
        //_shortFightCamera.gameObject.SetActive(value);
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
        if(_stateMachine.EnemiesNearby.Count < 2)
        {
            _shortFightCamera.gameObject.SetActive(value);
        }

    }

    public void SetAttackCameraState(bool value, string attackType)
    {
        _attackCamera.gameObject.SetActive(value);
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
                if(_stateMachine.EnemiesNearby.Count > 1)
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
