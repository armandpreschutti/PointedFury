using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTester : MonoBehaviour
{
    [SerializeField] Animator _anim;
    [SerializeField] GameObject _target;
    [SerializeField] int hitType;
    private void Start()
    {
        _anim = GetComponent<Animator>();
        _target = GameObject.Find("Player").gameObject;
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

    
    private void Update()
    {
        
    }
}
