using UnityEngine;
using Cinemachine;
using static UnityEngine.Rendering.DebugUI;
using DG;

public class PlayerCameraController : MonoBehaviour
{
    private StateMachine _stateMachine;
    public Transform _followTarget;

    // cinemachine
    private const float _threshold = 0.01f;
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;
    private float _pitchResetTime;


    [SerializeField] CinemachineVirtualCamera _freeRoamCamera;
    [SerializeField] CinemachineVirtualCamera _fightCamera;
    
    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject CinemachineCameraTarget;

    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 70.0f;

    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -30.0f;

    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float CameraAngleOverride = 0.0f;

    [Tooltip("For locking the camera position on all axis")]
    public bool LockCameraPosition = false;

    public float YAxisSensitivity;
    public float XAxisSensitivity;

    public float PitchResetDelay;
    public float FightPitchAngle;
    public float FreeRoamPitchAngle;
    public float PitchResetSpeed;


    private void Awake()
    {
        _stateMachine = GetComponent<StateMachine>();
        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
        _pitchResetTime = PitchResetDelay;
    }

    private void OnEnable()
    {
        RecenterCameraYaw();
    }

    private void OnDisable()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        _fightCamera = GameObject.Find("PlayerFightCamera").GetComponent<CinemachineVirtualCamera>();
        _fightCamera.Follow = _followTarget;
        _freeRoamCamera = GameObject.Find("PlayerFreeRoamCamera").GetComponent<CinemachineVirtualCamera>();
        _freeRoamCamera.Follow = _followTarget;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CameraRotation();
        SetFightCamera();
        CameraPitchPositioningLoop();
    }
    public void SetFightCamera()
    {
        _fightCamera.gameObject.SetActive(_stateMachine.IsFighting);        
    }

    private void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if (_stateMachine.LookInput.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            float deltaTimeMultiplier = Time.deltaTime;

            _cinemachineTargetYaw += (_stateMachine.LookInput.x * XAxisSensitivity) * Time.deltaTime;
            _cinemachineTargetPitch += (_stateMachine.LookInput.y *YAxisSensitivity) * Time.deltaTime;
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    public void SetFightCamera(bool value) 
    {
        _fightCamera.gameObject.SetActive(value);
    }

    private void RecenterCameraYaw()
    {
        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
    }
   
    public void CameraPitchPositioningLoop()
    {

         if (_stateMachine.LookInput == Vector2.zero && (_cinemachineTargetPitch > FightPitchAngle + .1 || _cinemachineTargetPitch < FightPitchAngle - .1))
            {
              
                _pitchResetTime -= Time.deltaTime;

                // Check if the timer has reached zero
                if (_pitchResetTime <= 0)
                {
                    float resetDuration = 0f;
                    resetDuration += Time.deltaTime;
                    _cinemachineTargetPitch = Mathf.Lerp(_cinemachineTargetPitch, _stateMachine.IsFighting ? FightPitchAngle : FreeRoamPitchAngle, resetDuration * PitchResetSpeed);
                }
            }
            else
            {
                _pitchResetTime = PitchResetDelay;
                return;
            }
       /* if(_stateMachine.IsFighting)
        {
            if (_stateMachine.LookInput == Vector2.zero && (_cinemachineTargetPitch > PitchResetAngle + .1 || _cinemachineTargetPitch < PitchResetAngle - .1))
            {
              
                _pitchResetTime -= Time.deltaTime;

                // Check if the timer has reached zero
                if (_pitchResetTime <= 0)
                {
                    float resetDuration = 0f;
                    resetDuration += Time.deltaTime;
                    _cinemachineTargetPitch = Mathf.Lerp(_cinemachineTargetPitch, PitchResetAngle, resetDuration * PitchResetSpeed);
                }
            }
            else
            {
                _pitchResetTime = PitchResetDelay;
                return;
            }
        }
        else
        {
            _pitchResetTime = 0;
        }*/
        
       
    }

}
