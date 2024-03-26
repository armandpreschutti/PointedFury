using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAttackState : BaseState
{
    public LightAttackState(StateMachine currentContext, StateFactory stateFactory)
    : base(currentContext, stateFactory) { }


    public override void EnterState()
    {
        SetAttackType();
        //Debug.LogWarning("Player has entered LIGHT ATTACK state");
        Ctx.DebugCurrentSubState = $"Light Attack State ({Ctx.AttackType})";

        Ctx.Animator.Play($"LightAttack{Ctx.AttackType}", 0, 0);
        Ctx.IsLightAttacking = true;
        Ctx.IsAttacking = true;
        Ctx.IsLightAttackPressed = false;
        Ctx.IsComboAttacking = false;
        Ctx.IsFighting = true;
        if (Ctx.AttackType < 7)
        {
            Ctx.CanComboAttack = true;
        }
        Ctx.OnLightAttack?.Invoke(true);
    }

    public override void UpdateState()
    {
        //Debug.Log("LIGHT ATTACK state is currently active");

        CheckSwitchStates();

        Ctx.SetAttackDirection();
        if (Ctx.IsCharging)
        {
            Ctx.LightAttackMovement();
        }
        if (Ctx.IsLightAttackPressed && /*!Ctx.IsHurt */ Ctx.CanComboAttack)
        {
            Ctx.IsComboAttacking = true;
            Ctx.CanComboAttack = false;
        }
    }

    public override void ExitState()
    {
        //Debug.LogWarning("Player has exited LIGHT ATTACK state");

        Ctx.IsAttacking = false;
        Ctx.OnLightAttack?.Invoke(false);
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsLightAttacking)
        {
            if (Ctx.IsComboAttacking)
            {
                SwitchState(Factory.LightAttack());
            }
            else
            {
                Ctx.FightTimeoutActive = true;
                Ctx.FightTimeoutDelta = Ctx.AttackTimeout;
                if (Ctx.MoveInput != Vector2.zero)
                {
                    SwitchState(Factory.Move());
                }
                else
                {
                    SwitchState(Factory.Idle());
                }
            }

        }
        if (Ctx.IsHitLanded)
        {
            SwitchState(Factory.Hurt());
            Ctx.IsLightAttacking = false;
        }
        if (Ctx.IsDodgeSuccess)
        {
            Ctx.IsLightAttacking = false;
            SwitchState(Factory.Dodge());
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
            default:
                break;
        }
    }
}
