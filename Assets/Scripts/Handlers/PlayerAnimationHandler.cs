
using Cinemachine.Utility;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using DG.Tweening;
using static UnityEngine.Rendering.DebugUI;

public class PlayerAnimationHandler : MonoBehaviour
{
    [SerializeField] StateMachine _stateMachine;
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
    [SerializeField] private int _animationIDAttackType;
    [SerializeField] private int _attackType;
    [SerializeField] private bool _isFighting;
    [SerializeField] private float _debugTime;

    private void Awake()
    {
        SetComponents();
        AssignAnimationIDs();
    }

    private void OnEnable()
    {
        _stateMachine.OnGrounded += SetGroundedAnimation;
        _stateMachine.OnJump += SetJumpAnimation;
        _stateMachine.OnFall += SetFallAnimation;
        _stateMachine.OnFight += SetFightAnimation;
        _stateMachine.OnAttack += SetAttackAnimation;
    }

    private void OnDisable()
    {
        _stateMachine.OnGrounded -= SetGroundedAnimation;
        _stateMachine.OnJump -= SetJumpAnimation;
        _stateMachine.OnFall -= SetFallAnimation;
        _stateMachine.OnFight -= SetFightAnimation;
        _stateMachine.OnAttack -= SetAttackAnimation;
    }

    private void Update()
    {
        SetMovementAnimationValues();
        SetMovementAnimationSpeed();
        SetStateAnimationLayer();
    }

    public void SetComponents()
    {
        _stateMachine = GetComponent<StateMachine>();
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
        _animIDLightAttack = Animator.StringToHash("Attack");
        _animationIDAttackType = Animator.StringToHash("AttackType");
    }

    private void SetGroundedAnimation(bool value)
    {
        _anim.SetBool(_animIDGrounded, value);
    }

    private void SetJumpAnimation(bool value)
    {
        _anim.rootPosition = transform.position;
        _anim.SetBool(_animIDJump, value);
    }

    private void SetFallAnimation(bool value)
    { 
        _anim.SetBool(_animIDFall, value);
    }

    private void SetAttackAnimation(bool value)
    {
        _anim.SetInteger(_animationIDAttackType, _stateMachine.AttackType);
        _anim.SetBool(_animIDLightAttack, value);
    }

    private void SetFightAnimation(bool value)
    {
        _isFighting = value;
        _attackType = 0;
    }
    private void SetMovementAnimationValues()
    {
        if(_stateMachine.IsFighting)
        {
          //  _anim.SetFloat(_animIDInputX, _playerStateMachine.EnemyRelativeInput().x);
            //_anim.SetFloat(_animIDInputY, _playerStateMachine.EnemyRelativeInput().y);
        }
        else
        {
            _anim.SetFloat(_animIDInputX, _stateMachine.MoveInput.x);
            _anim.SetFloat(_animIDInputY, _stateMachine.MoveInput.y);
        }
        
    }

    private void SetMovementAnimationSpeed()
    {
        if(_stateMachine != null)
        {
            _animationBlend = Mathf.Lerp(_animationBlend, _stateMachine.TargetSpeed, Time.deltaTime * _stateMachine.SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;
            _anim.SetFloat(_animIDSpeed, _animationBlend);
        }
        else
        {
            return;
        }        
    }
    public void SetStateAnimationLayer()
    {
        _debugTime = LerpBetweenValues(_isFighting, _debugTime);
        _anim.SetLayerWeight(1, _debugTime);        
    }
   

    float LerpBetweenValues(bool condition, float value)
    {
        float debug = value;
        
        if (condition)
        {
            if(value < 1f)
            {
                debug += Time.deltaTime * 2f;
            }
            else
            {
                debug = 1f;
            }
            
        }
        else
        {
            if(value > 0f)
            {
                debug -= Time.deltaTime * 2f;
            }
            else
            {
                debug = 0f;
            }
            
        }
        return debug;
    }
}
