using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DebugTester : MonoBehaviour
{
    [SerializeField] Animator _anim;
    [SerializeField] GameObject _target;
    [SerializeField] int hitType;
    [SerializeField] int randomChance;
    [SerializeField] Vector2 randomInput;
    [SerializeField] CharacterController characterController;

    [SerializeField] float targetDistance;
    [SerializeField] Vector3 moveDirection;
    [SerializeField] Vector3 inputDirection;




    private void Start()
    {
        _anim = GetComponent<Animator>();
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

        inputDirection = forward * randomInput.y + right * randomInput.x;
        inputDirection = inputDirection.normalized;

        transform.DOLookAt(_target.transform.position, .2f);
        characterController.Move(inputDirection * Time.deltaTime);
     
    }
    public IEnumerator GetRandomDirection()
    {
        yield return new WaitForSeconds(5);
        Debug.Log("New chance assigned");
        randomChance = Random.Range(0, 2);
        randomInput = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2));
        StartCoroutine(GetRandomDirection());
    }
    public void PlayHurtAnimation(int attackType)
    {
        transform.DOLookAt(_target.transform.position, .01f);

        switch (attackType)
        {
            case 0:
                hitType = 0;
                _anim.Play("Hurt1");
                break;
            case 1:
                hitType = 1;
                _anim.Play("Hurt2");
                break;
            case 2:
                hitType = 2;
                _anim.Play("Hurt3");
                break;
            default:
                break;
        }

    }

    
   
   

   

   
}
