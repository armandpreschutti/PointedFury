using UnityEngine;
using Cinemachine;
using static UnityEngine.Rendering.DebugUI;
using DG;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;

public class PlayerCameraController : MonoBehaviour
{
    private StateMachine _stateMachine;
    private EnemyDetectionHandler _enemyDetectionHandler;
    public Transform _followTarget;
    public LayerMask occlussionLayer;

    // cinemachine
    private const float _threshold = 0.01f;
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;
    private float _pitchResetTime;
    private float _yawResetTime;

    [SerializeField] CinemachineVirtualCamera _freeRoamCamera;
    [SerializeField] CinemachineVirtualCamera _groupFightCamera;
    [SerializeField] CinemachineVirtualCamera _individualFightCamera;
    
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

    public float FightSensitivity;
    public float FreeRoamSensitivity;

    public float YAxisSensitivity;
    public float XAxisSensitivity;

    public float PitchResetDelay;
    public float FightPitchAngle;
    public float FreeRoamPitchAngle;
    public float PitchResetSpeed;

    public float YawResetDelay;
    public float FighYawAngle;
    public float FreeRoamYawAngle;
    public float YawResetSpeed;


    private void Awake()
    {
        _stateMachine = GetComponent<StateMachine>();
        _enemyDetectionHandler = GetComponentInChildren<EnemyDetectionHandler>();
        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
        _pitchResetTime = PitchResetDelay;
    }
    private void OnEnable()
    {
        EnemyManagementSystem.OnZoneEntered += SetGroupFightCameraTarget;
        EnemyManagementSystem.OnZoneEntered += SetGroupFightCameraState;
        EnemyManagementSystem.OnZoneEnemiesCleared += SetGroupFightCameraTarget;
    }
    private void OnDisable()
    {
        EnemyManagementSystem.OnZoneEntered += SetGroupFightCameraTarget;
        EnemyManagementSystem.OnZoneEntered -= SetGroupFightCameraState;
        EnemyManagementSystem.OnZoneEnemiesCleared += SetGroupFightCameraTarget;
    }

    // Start is called before the first frame update
    void Start()
    {
        _groupFightCamera = GameObject.Find("GroupFightCamera").GetComponent<CinemachineVirtualCamera>();
        _groupFightCamera.Follow = _followTarget;
        _freeRoamCamera = GameObject.Find("FreeRoamCamera").GetComponent<CinemachineVirtualCamera>();
        _freeRoamCamera.Follow = _followTarget;
        _groupFightCamera.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CameraRotation();
        SetCameraSensitity();
        SetGroupFightCameraState(_stateMachine.IsFighting);
        CameraPitchPositioningLoop();
       // UnblockCamera();
    }
 
    public void SetGroupFightCameraState(bool value)
    {
        
        _groupFightCamera.gameObject.SetActive(value);
        
    }
    public void SetGroupFightCameraTarget(bool value)
    {
        _groupFightCamera.LookAt = value ? GameObject.Find("TargetGroup").transform : transform.Find("TempTarget");
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
                _cinemachineTargetPitch = Mathf.Lerp(_cinemachineTargetPitch, _stateMachine.IsFighting ? FightPitchAngle : FreeRoamPitchAngle, resetDuration * PitchResetSpeed);
            }
        }
        else
        {
            _pitchResetTime = PitchResetDelay;
            return;
        }
    }
    public void UnblockCamera()
    {
        // Get the main camera
        Camera mainCamera = Camera.main;

        // Get the center of the screen in viewport coordinates (0.5, 0.5)
        Vector3 viewportCenter = new Vector3(0.5f, 0.5f, 0f);

        // Convert the viewport coordinates to a ray
        Ray ray = mainCamera.ViewportPointToRay(viewportCenter);

        // Declare a RaycastHit variable to store information about the hit
        RaycastHit hit;

        // Check if the ray hits any game object
        if (Physics.Raycast(ray, out hit, 20f, occlussionLayer))
        {
            Debug.Log(hit.transform.name);
            _cinemachineTargetYaw += 1f;
        }
    }
}
