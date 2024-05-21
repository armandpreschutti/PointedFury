using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAttackState : BaseState
{
    public LightAttackState(StateMachine currentContext, StateFactory stateFactory)
    : base(currentContext, stateFactory) { }


    public override void EnterState()
    {
        //Debug.LogWarning("Player has entered LIGHT ATTACK state");

        SetAttackType();
        Ctx.SetAttackDirection();
        Ctx.Animator.SetBool(Ctx.AnimIDLightAttack, true);
        Ctx.Animator.SetInteger(Ctx.AnimIDLightAttackID, Ctx.LightAttackID);
        Ctx.IsAttacking = true;
        Ctx.IsLightAttackPressed = false;
        Ctx.IsFighting = true;
        Ctx.OnFight?.Invoke(true);
        Ctx.OnLightAttack?.Invoke(true, "Light");
        Ctx.OnAttack?.Invoke(true);
        //Ctx.IsBlockPressed = false;
    }

    public override void UpdateState()
    {
        //Debug.Log("LIGHT ATTACK state is currently active");
        Ctx.DebugCurrentSubState = $"Light Attack {Ctx.LightAttackID} State";
        CheckSwitchStates();

        if (Ctx.IsCharging)
        {
           //Ctx.SetAttackDirection();
            Ctx.AttackMovement();
        }
    }

    public override void ExitState()
    {
        //Debug.LogWarning("Player has exited LIGHT ATTACK state");

        Ctx.Animator.SetBool(Ctx.AnimIDLightAttack, false);
        Ctx.IsAttacking = false;
        Ctx.OnLightAttack?.Invoke(false, "Light");
        Ctx.OnAttack?.Invoke(false);
        Ctx.IsCharging = false;
        Ctx.IsParryable = false;
        Ctx.IsEvadePressed = false;
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsDead)
        {
            if (!Ctx.IsAttacking)
            {
                if (Ctx.IsParried)
                {
                    SwitchState(Factory.Stunned());
                }
                else if (Ctx.IsLightAttackPressed)
                {
                    SwitchState(Factory.LightAttack());
                }
                else if (Ctx.IsHeavyAttackPressed)
                {
                    SwitchState(Factory.HeavyAttack());
                }
                else
                {
                    if (Ctx.IsBlockPressed)
                    {
                        SwitchState(Factory.Block());
                    }
                    else
                    {
                        SwitchState(Factory.PostAttack());
                    }
                }
            }
            else if (Ctx.IsLightHitLanded)
            {
                SwitchState(Factory.Hurt());
            }
            else if (Ctx.IsHeavyHitLanded)
            {
                SwitchState(Factory.Hurt());
            }
            else if (Ctx.IsParried)
            {
                SwitchState(Factory.Stunned());
            }
            else if (Ctx.IsParrySucces)
            {
                SwitchState(Factory.Parry());
            }
            else if (Ctx.IsEvadeSucces)
            {
                SwitchState(Factory.Evade());
            }
            else if (Ctx.IsDashPressed)
            {
                SwitchState(Factory.Dash());
            }
        }
       
    }

    public override void InitializeSubStates()
    {

    }

    public void SetAttackType()
    {
        Ctx.AttackType = "Light";
        switch (Ctx.LightAttackID)
        {
            case 0:
                Ctx.LightAttackID = 1;
                break;
            case 1:
                Ctx.LightAttackID = 2;
                break;
            case 2:
                Ctx.LightAttackID = 3;
                break;
            case 3:
                Ctx.LightAttackID = 4;
                break;
            case 4:
                Ctx.LightAttackID = 5;
                break;
            case 5:
                Ctx.LightAttackID = 1;
                break;
            default:
                break;
        }
    }
}
