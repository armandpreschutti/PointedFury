
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
    [SerializeField] private int _animIDFight;
    [SerializeField] private int _animIDInputX;
    [SerializeField] private int _animIDInputY;
    [SerializeField] private int _animIDLightAttack1;
    [SerializeField] private int _animIDLightAttack2;
    [SerializeField] private int _animIDLightAttack3;
    [SerializeField] private int _animIDCombo;
    [SerializeField] private int _animationIDAttackType;
    [SerializeField] private bool _isFighting;
    [SerializeField] private float _transitionTime;

    private void Awake()
    {
        SetComponents();
        AssignAnimationIDs();
    }

    private void OnEnable()
    {
        _stateMachine.OnGrounded += SetGroundedAnimation;
        _stateMachine.OnFight += SetFightAnimation;
        _stateMachine.OnLightAttack1 += SetLightAttack1Animation;
        _stateMachine.OnLightAttack2 += SetLightAttack2Animation;
        _stateMachine.OnLightAttack3 += SetLightAttack3Animation;
        //_stateMachine.OnComboAttack += SetComboAnimation;
    }

    private void OnDisable()
    {
        _stateMachine.OnGrounded -= SetGroundedAnimation;
        _stateMachine.OnFight -= SetFightAnimation;
        _stateMachine.OnLightAttack1 -= SetLightAttack1Animation;
        _stateMachine.OnLightAttack2 -= SetLightAttack2Animation;
        _stateMachine.OnLightAttack3 -= SetLightAttack3Animation;
        //_stateMachine.OnComboAttack -= SetComboAnimation;
    }

    private void Update()
    {
        SetMovementAnimationValues();
        SetMovementAnimationSpeed();
       // SetStateAnimationLayer();
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
        _animIDFight = Animator.StringToHash("Fight");
        _animIDInputX = Animator.StringToHash("InputX");
        _animIDInputY = Animator.StringToHash("InputY");
        _animIDLightAttack1 = Animator.StringToHash("LightAttack1");
        _animIDLightAttack2 = Animator.StringToHash("LightAttack2");
        _animIDLightAttack3 = Animator.StringToHash("LightAttack3");
    }

    private void SetGroundedAnimation(bool value)
    {
        _anim.SetBool(_animIDGrounded, value);
    }

    private void SetLightAttack1Animation(bool value)
    {
        _anim.SetBool(_animIDLightAttack1, value);
    }
    private void SetLightAttack2Animation(bool value)
    {
        _anim.SetBool(_animIDLightAttack2, value);
    }
    private void SetLightAttack3Animation(bool value)
    {
        _anim.SetBool(_animIDLightAttack3, value);
    }

    private void SetFightAnimation(bool value)
    {
        _isFighting = value;
    }

    private void SetComboAnimation(bool value)
    {
        _anim.SetBool(_animIDCombo, value);
    }
    private void SetMovementAnimationValues()
    {
        _anim.SetFloat(_animIDInputX, _stateMachine.MoveInput.x);
        _anim.SetFloat(_animIDInputY, _stateMachine.MoveInput.y);
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
        _transitionTime = LerpBetweenValues(_isFighting, _transitionTime);
        _anim.SetLayerWeight(1, _transitionTime);        
    }
   

    float LerpBetweenValues(bool condition, float value)
    {
        float debug = value;
        
        if (condition)
        {
            if(value < 1f)
            {
                debug += Time.deltaTime * 4f;
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
                debug -= Time.deltaTime * 4f;
            }
            else
            {
                debug = 0f;
            }
            
        }
        return debug;
    }
}
