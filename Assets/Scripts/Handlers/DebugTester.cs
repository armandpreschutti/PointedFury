using DG.Tweening;
using System;
using System.Collections;

using UnityEngine;

public class DebugTester : MonoBehaviour
{
    [SerializeField] Animator _anim;
    [SerializeField] GameObject _target;
    [SerializeField] int _hitType;
    [SerializeField] int _randomChance;
    [SerializeField] Vector2 _randomInput;
    [SerializeField] CharacterController _characterController;
    [SerializeField] float _targetDistance;
    [SerializeField] Vector3 _moveDirection;
    [SerializeField] Vector3 _inputDirection;
   // [SerializeField] HealthSystem _healthSystem;

    public Action<int> OnHit;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        //_healthSystem = GetComponent<HealthSystem>();
        _characterController= GetComponent<CharacterController>();
    }
    private void OnEnable()
    {
        OnHit += PlayHurtAnimation;
       // _healthSystem.OnDeath += PlayDeathAnimation;
    }
    private void OnDisable()
    {
        OnHit += PlayHurtAnimation;
        //_healthSystem.OnDeath -= PlayDeathAnimation;
    }

    private void Start()
    {
        _target = GameObject.Find("Player").gameObject;
        StartCoroutine(GetRandomDirection());
    }

    void Update()
    {
        
        var forward = transform.forward;
        var right = transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        _inputDirection = forward * _randomInput.y + right * _randomInput.x;
        _inputDirection = _inputDirection.normalized;

        transform.DOLookAt(_target.transform.position, .2f);
        _characterController.Move(_inputDirection * Time.deltaTime);
     
    }
    public IEnumerator GetRandomDirection()
    {
        yield return new WaitForSeconds(5);
        Debug.Log("New chance assigned");
        _randomChance = UnityEngine.Random.Range(0, 2);
        _randomInput = new Vector2(UnityEngine.Random.Range(-1, 2), UnityEngine.Random.Range(-1, 2));
        StartCoroutine(GetRandomDirection());
    }

    public void TakeHit(int attackType)
    {
        OnHit?.Invoke(attackType);
    }

    public void PlayHurtAnimation(int attackType)
    {
        transform.DOLookAt(_target.transform.position, .01f);

         switch (attackType)
         {
            case 0:
                _hitType = 0;
                _anim.Play("Hurt1");
                break;
            case 1:
                _hitType = 1;
                _anim.Play("Hurt2");
                break;
            case 2:
                _hitType = 2;
                _anim.Play("Hurt3");
                break;
            default:
                break;
         }
    }

    public void PlayDeathAnimation()
    {
        _anim.Play("Death");
        GetComponent<CharacterController>().enabled = false;
    }
    
   
   

   

   
}
