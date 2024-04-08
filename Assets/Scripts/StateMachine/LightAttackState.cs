using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAttackState : BaseState
{
    public LightAttackState(StateMachine currentContext, StateFactory stateFactory)
    : base(currentContext, stateFactory) { }


    public override void EnterState()
    {
        Debug.LogWarning("Player has entered LIGHT ATTACK state");

        SetAttackType();
        Ctx.Animator.Play($"LightAttack{Ctx.AttackType}", 0, 0);
        Ctx.Animator.SetBool(Ctx.AnimIDLightAttack, true);
        Ctx.IsAttacking = true;
        Ctx.IsLightAttackPressed = false;
        Ctx.IsParryable = true;
        Ctx.OnLightAttack?.Invoke(true);
        Ctx.IsBlockPressed = false;
    }

    public override void UpdateState()
    {
        //Debug.Log("LIGHT ATTACK state is currently active");
        Ctx.DebugCurrentSubState = $"Light Attack {Ctx.AttackType} State";
        CheckSwitchStates();

        if (Ctx.IsCharging)
        {
            Ctx.SetAttackDirection();
            Ctx.AttackMovement();
        }
    }

    public override void ExitState()
    {
        //Debug.LogWarning("Player has exited LIGHT ATTACK state");

        Ctx.Animator.SetBool(Ctx.AnimIDLightAttack, false);
        Ctx.IsAttacking = false;
        Ctx.OnLightAttack?.Invoke(false);
        Ctx.IsCharging = false;
        Ctx.IsParryable = false;
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsAttacking)
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
        if (Ctx.IsHitLanded)
        {
            SwitchState(Factory.Hurt());
        }
        if (Ctx.IsParried)
        {
            SwitchState(Factory.Stunned());
        }
        if (Ctx.IsParrySucces)
        {
            SwitchState(Factory.Parry());
        }
    }

    public override void InitializeSubStates()
    {

    }

    public void SetAttackType()
    {
        switch (Ctx.AttackType)
        {

            case 0:
                Ctx.AttackType = 1;
                break;
            case 1:
                Ctx.AttackType = 2;
                break;
            case 2:
                Ctx.AttackType = 3;
                break;
            case 3:
                Ctx.AttackType = 4;
                break;
            case 4:
                Ctx.AttackType = 5;
                break;
            case 5:
                Ctx.AttackType = 6;
                break;
            case 6:
                Ctx.AttackType = 7;
                break;
            case 7:
                Ctx.AttackType = 1;
                break;
            default:
                break;
        }
    }
}
