using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackIndicationHandler : MonoBehaviour
{
    public StateMachine _stateMachine;
    public GameObject _attackIndicator;
    public HealthSystem _healthSystem;
    public bool showParryIndicator;
    public bool showEvadeIndicator;

    private void OnEnable()
    {
        if (GetComponentInParent<HealthSystem>() != null)
        {
            GetComponent<HealthSystem>().OnDeath += DisableIndicator;
        }
    }

    private void OnDisable()
    {
        if (GetComponentInParent<HealthSystem>() != null)
        {
            GetComponent<HealthSystem>().OnDeath -= DisableIndicator;
        }
    }

    private void Update()
    {
        
        if(_stateMachine.AttackType == "Heavy" && _stateMachine.IsParryable && showParryIndicator)
        {
            _attackIndicator.SetActive(true);
            _attackIndicator.GetComponent<MeshRenderer>().material.color = Color.red;
        }
      /*  else if(_stateMachine.IsNearDeath == true)
        {
            _attackIndicator.SetActive(true);
            _attackIndicator.GetComponent<MeshRenderer>().material.color = Color.yellow;
        }*/
        else
        {
            _attackIndicator.SetActive(false);
        }
        

    }

    public void DisableIndicator(/*float damage, string type*/)
    {
        Destroy(_attackIndicator);
        Destroy(this);
    }
}
