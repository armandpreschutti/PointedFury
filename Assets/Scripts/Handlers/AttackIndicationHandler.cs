using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackIndicationHandler : MonoBehaviour
{
    public StateMachine _stateMachine;
    public GameObject[] _indicators;

    public Material _parryMaterial;
    public Material _evadeMaterial;
    public HealthSystem _healthSystem;
    public bool showParryIndicator;
    public bool showEvadeIndicator;

    private void OnEnable()
    {
        if (GetComponentInParent<HealthSystem>() != null)
        {
            GetComponent<HealthSystem>().OnDeath += DisableIndicator;
        }
        _stateMachine.OnHeavyAttack += SetIndicator;
        _stateMachine.OnParry += SetIndicator;
    }

    private void OnDisable()
    {
        if (GetComponentInParent<HealthSystem>() != null)
        {
            GetComponent<HealthSystem>().OnDeath -= DisableIndicator;
        }
        _stateMachine.OnHeavyAttack -= SetIndicator;
        _stateMachine.OnParry -= SetIndicator;
    }

    public void DisableIndicator(/*float damage, string type*/)
    {
        for (int i = 0; i < _indicators.Length; i++)
        {
            SkinnedMeshRenderer renderer = _indicators[i].GetComponent<SkinnedMeshRenderer>();
            Material[] materials = renderer.materials;
            if (materials.Length > 1)
            {
                materials[1] = materials[0];
                renderer.materials = materials; // Reassign the modified materials array
            }
        }
    }

    public void SetIndicator(bool value, string attackType)
    {
        if (value)
        {
            Debug.Log("Trying to set Parry Material");
            for (int i = 0; i < _indicators.Length; i++)
            {
                SkinnedMeshRenderer renderer = _indicators[i].GetComponent<SkinnedMeshRenderer>();
                Material[] materials = renderer.materials;
                if (materials.Length > 1)
                {
                    if(attackType == "Heavy")
                    {
                        materials[1] = _parryMaterial;
                        renderer.materials = materials; // Reassign the modified materials array
                    }
                    if(attackType == "Parry")
                    {
                        materials[1] = _evadeMaterial;
                        renderer.materials = materials; // Reassign the modified materials array
                    }

                }
            }
        }
        else
        {
            Debug.Log("Trying to set Original Material");
            for (int i = 0; i < _indicators.Length; i++)
            {
                SkinnedMeshRenderer renderer = _indicators[i].GetComponent<SkinnedMeshRenderer>();
                Material[] materials = renderer.materials;
                if (materials.Length > 1)
                {
                    materials[1] = materials[0];
                    renderer.materials = materials; // Reassign the modified materials array
                }
            }
        }
    }
}
