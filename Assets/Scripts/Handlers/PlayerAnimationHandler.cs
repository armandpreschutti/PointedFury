
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    [SerializeField] PlayerStateMachine _playerStateMachine;
    [SerializeField] Animator _animator;

    [SerializeField] private float _animationBlend;
    [SerializeField] private int _animIDSpeed;
    [SerializeField] private int _animIDGrounded;
    [SerializeField] private int _animIDJump;
    [SerializeField] private int _animIDFall;
    [SerializeField] private int _animIDFight;

    private void Awake()
    {
        SetComponents();
        AssignAnimationIDs();
    }

    private void OnEnable()
    {
        _playerStateMachine.OnGrounded += SetGroundedAnimation;
        _playerStateMachine.OnJump += SetJumpAnimation;
        _playerStateMachine.OnFall += SetFallAnimation;
        _playerStateMachine.OnFight += SetFightAnimation;
    }

    private void OnDisable()
    {
        _playerStateMachine.OnGrounded -= SetGroundedAnimation;
        _playerStateMachine.OnJump -= SetJumpAnimation;
        _playerStateMachine.OnFall -= SetFallAnimation;
        _playerStateMachine.OnFight -= SetFightAnimation;
    }

    private void Update()
    {
        if (!_playerStateMachine.IsFighting)
        {
            _animationBlend = Mathf.Lerp(_animationBlend, _playerStateMachine.TargetSpeed, Time.deltaTime * _playerStateMachine.SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;
            //_animationBlend = _playerStateMachine.Speed;
            _animator.SetFloat(_animIDSpeed, _animationBlend);
        }
        else
        {
            _animator.SetFloat(_animIDSpeed, 0f);
        }
       
    }

    public void SetComponents()
    {
        _playerStateMachine = GetComponent<PlayerStateMachine>();
        _animator = GetComponent<Animator>();
    }

    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFall = Animator.StringToHash("InAir");
        _animIDFight = Animator.StringToHash("Fight");
    }

    private void SetGroundedAnimation(bool value)
    {
        _animator.SetBool(_animIDGrounded, value);
    }

    private void SetJumpAnimation(bool value)
    {
        _animator.SetBool(_animIDJump, value);
    }

    private void SetFallAnimation(bool value)
    {
        _animator.SetBool(_animIDFall, value);
    }
    private void SetFightAnimation(bool value)
    {
        _animator.SetBool(_animIDFight, value);
    }
}
