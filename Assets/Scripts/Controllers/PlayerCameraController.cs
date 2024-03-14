using UnityEngine;
using Cinemachine;

public class PlayerCameraController : MonoBehaviour
{
    private StateMachine _stateMachine;
    [SerializeField] CinemachineVirtualCamera _freeCamera;
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

    // cinemachine
    private const float _threshold = 0.01f;
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    private void Awake()
    {
        _stateMachine = GetComponent<StateMachine>();
        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
    }

    private void OnEnable()
    {
        _stateMachine.OnFight += SetFightCamera;
    }

    private void OnDisable()
    {
        _stateMachine.OnFight -= SetFightCamera;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CameraRotation();
       /* if (!_playerStateMachine.IsFighting)
        {
            CameraRotation();
        }
        else
        {
            return;
        }*/
    }

    private void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if (_stateMachine.LookInput.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            float deltaTimeMultiplier = Time.deltaTime;

            _cinemachineTargetYaw += _stateMachine.LookInput.x * Time.deltaTime;
            _cinemachineTargetPitch += _stateMachine.LookInput.y * Time.deltaTime;
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

}
