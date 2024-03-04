
using Cinemachine.Utility;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    [SerializeField] PlayerStateMachine _playerStateMachine;
    [SerializeField] Animator _anim;
    

    [SerializeField] private float _animationBlend;
    [SerializeField] private int _animIDSpeed;
    [SerializeField] private int _animIDGrounded;
    [SerializeField] private int _animIDJump;
    [SerializeField] private int _animIDFall;
    [SerializeField] private int _animIDFight;
    [SerializeField] private int _animIDInputX;
    [SerializeField] private int _animIDInputY;
    [SerializeField] private int _animIDLightAttack;

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
        _playerStateMachine.OnFight += SetFightLayer;
        _playerStateMachine.OnLightAttack += SetLightAttackAnimation;
        //_playerStateMachine.OnFight += SetAvatarMaskWeight;
    }

    private void OnDisable()
    {
        _playerStateMachine.OnGrounded -= SetGroundedAnimation;
        _playerStateMachine.OnJump -= SetJumpAnimation;
        _playerStateMachine.OnFall -= SetFallAnimation;
        _playerStateMachine.OnFight -= SetFightLayer;
        //_playerStateMachine.OnFight -= SetAvatarMaskWeight;
    }

    private void Update()
    {
        SetAnimationSpeed();
        //SetFightStrafe(_playerStateMachine.MoveInput.normalized);
    }

    public void SetComponents()
    {
        _playerStateMachine = GetComponent<PlayerStateMachine>();
        _anim = GetComponent<Animator>();
    }

    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFall = Animator.StringToHash("Fall");
        _animIDFight = Animator.StringToHash("Fight");
        _animIDInputX = Animator.StringToHash("InputX");
        _animIDInputY = Animator.StringToHash("InputY");
        _animIDLightAttack = Animator.StringToHash("LightAttack");
    }

    private void SetGroundedAnimation(bool value)
    {
        _anim.SetBool(_animIDGrounded, value);
    }

    private void SetJumpAnimation(bool value)
    {
        _anim.SetBool(_animIDJump, value);
    }

    private void SetFallAnimation(bool value)
    {
        _anim.SetBool(_animIDFall, value);
    }

    private void SetLightAttackAnimation(bool value)
    {
        _anim.SetBool(_animIDLightAttack, value);
    }
    private void SetFightLayer(bool value)
    {
        if (value)
        {
            _anim.SetLayerWeight(1, 1f);
        }
        else
        {
            _anim.SetLayerWeight(1, 0f);
        }
    }

    private void SetFightStrafe(Vector2 moveInput)
    {
        _anim.SetFloat(_animIDInputX, moveInput.x);
        _anim.SetFloat(_animIDInputY, moveInput.y);
    }

    private void SetAnimationSpeed()
    {
        if(_playerStateMachine != null)
        {
            /* if (!_playerStateMachine.IsFighting)
             {
                 _animationBlend = Mathf.Lerp(_animationBlend, _playerStateMachine.TargetSpeed, Time.deltaTime * _playerStateMachine.SpeedChangeRate);
                 if (_animationBlend < 0.01f) _animationBlend = 0f;
                 _anim.SetFloat(_animIDSpeed, _animationBlend);
             }
             else
             {
                 _anim.SetFloat(_animIDSpeed, 0f);
             }*/
            _animationBlend = Mathf.Lerp(_animationBlend, _playerStateMachine.TargetSpeed, Time.deltaTime * _playerStateMachine.SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;
            _anim.SetFloat(_animIDSpeed, _animationBlend);
        }
        else
        {
            return;
        }        
    }

    private void SetAvatarMaskWeight(bool value)
    {
        if (value)
        {
            _anim.SetLayerWeight(1, 1f);
            _anim.SetLayerWeight(2, 1f);
        }
        else
        {
            _anim.SetLayerWeight(1, 0f);
            _anim.SetLayerWeight(2, 0f);
        }
        
    }
}
