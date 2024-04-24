using UnityEngine;
using Cinemachine;


public class PlayerCameraController : MonoBehaviour
{
    private StateMachine _stateMachine;
    public Transform _followTarget;
    public LayerMask occlussionLayer;

    // cinemachine
    private const float _threshold = 0.01f;
    public float _cinemachineTargetYaw;
    public float _cinemachineTargetPitch;
    private float _pitchResetTime;
    private float _yawResetTime;

    [SerializeField] CinemachineVirtualCamera _freeRoamCamera;
    [SerializeField] CinemachineVirtualCamera _zoomedAttackCamera;
    [SerializeField] CinemachineVirtualCamera _shortFightCamera;
    [SerializeField] CinemachineVirtualCamera _longFightCamera;


    
    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject CinemachineCameraTarget;

    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 0f;
    public float FightTopClamp = 35f;
    public float FreeRoamTopClamp = 70f;

    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = 0f;
    public float FightBottomClamp = 35f;
    public float FreeRoamBottomClamp = 70f;

    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float CameraAngleOverride = 0.0f;

    [Tooltip("For locking the camera position on all axis")]
    public bool LockCameraPosition = false;
    public bool RepositionYaw = false;

    public float FightSensitivity;
    public float FreeRoamSensitivity;

    public float YAxisSensitivity;
    public float XAxisSensitivity;

    public float PitchResetDelay;
    public float FightPitchAngle;
    public float FreeRoamPitchAngle;
    public float PitchResetSpeed;

    public float YawResetDelay;
    public float FightYawAngle;
    public float FreeRoamYawAngle;
    public float YawResetSpeed;


    private void Awake()
    {
        _stateMachine = GetComponent<StateMachine>();

        _pitchResetTime = PitchResetDelay;
        _yawResetTime = YawResetDelay;
    }
    private void OnEnable()
    {
        EnemyManagementSystem.OnZoneEntered += SetCameraState;
       _stateMachine.OnFight += SetCameraState;
    }
    private void OnDisable()
    {
        EnemyManagementSystem.OnZoneEntered -= SetCameraState;
         _stateMachine.OnFight -= SetCameraState;
    }

    // Start is called before the first frame update    
    void Start()
    {
        //_cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
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
 
    public void SetCameraState(bool value)
    {
        
        //_shortFightCamera.gameObject.SetActive(value);
        if (value)
        {

            if (_stateMachine.EnemiesNearby.Count < 2)
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

    }
  

    public void SetCameraSensitity()
    {
        XAxisSensitivity = _stateMachine.IsFighting ? FightSensitivity : FreeRoamSensitivity;
        YAxisSensitivity = _stateMachine.IsFighting ? FightSensitivity : FreeRoamSensitivity;
        TopClamp = _stateMachine.IsFighting ? FightTopClamp : FreeRoamTopClamp;
        BottomClamp = _stateMachine.IsFighting ? FightBottomClamp : FreeRoamBottomClamp;
    }

    private void CameraRotation()
    {
        if (_stateMachine.LookInput.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            float deltaTimeMultiplier = Time.deltaTime;

            _cinemachineTargetYaw += (_stateMachine.LookInput.x * XAxisSensitivity) * Time.deltaTime;
            _cinemachineTargetPitch += (_stateMachine.LookInput.y * YAxisSensitivity) * Time.deltaTime;

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);
        }

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
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
