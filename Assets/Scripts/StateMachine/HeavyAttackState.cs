using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyAttackState : BaseState
{
    public HeavyAttackState(StateMachine currentContext, StateFactory stateFactory)
    : base(currentContext, stateFactory) { }


    public override void EnterState()
    {
        //Debug.LogWarning("Player has entered HEAVY ATTACK state");

        SetAttackType();

        Ctx.Animator.SetBool(Ctx.AnimIDHeavyAttack, true);
        Ctx.Animator.SetInteger(Ctx.AnimIDHeavyAttackID, Ctx.HeavyAttackID);
        Ctx.IsAttacking = true;
        Ctx.IsHeavyAttackPressed = false;
        Ctx.IsFighting = true;
        Ctx.OnFight?.Invoke(true);
        Ctx.OnHeavyAttack?.Invoke(true, "Heavy");
        Ctx.OnAttack?.Invoke(true);
        Ctx.IsBlockPressed = false;
    }

    public override void UpdateState()
    {
        //Debug.Log("LIGHT ATTACK state is currently active");
        Ctx.DebugCurrentSubState = $"Heavy Attack {Ctx.HeavyAttackID} State";
        CheckSwitchStates();

        if (Ctx.IsCharging)
        {
            Ctx.SetAttackDirection();
            Ctx.AttackMovement();
        }
    }

    public override void ExitState()
    {
        //Debug.LogWarning("Player has exited HEAVY ATTACK state");

        Ctx.Animator.SetBool(Ctx.AnimIDHeavyAttack, false);
        Ctx.IsAttacking = false;
        Ctx.OnHeavyAttack?.Invoke(false, "Heavy");
        Ctx.OnAttack?.Invoke(false);
        Ctx.IsCharging = false;
        Ctx.IsParryable = false;
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsAttacking)
        {
            if (Ctx.IsHeavyAttackPressed)
            {
                SwitchState(Factory.HeavyAttack());
            }
            else
            {
                SwitchState(Factory.PostAttack());
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
    }

    public override void InitializeSubStates()
    {

    }

    public void SetAttackType()
    {
        Ctx.AttackType = "Heavy";
        switch (Ctx.HeavyAttackID)
        {
            case 0:
                Ctx.HeavyAttackID = 1;
                break;
            case 1:
                Ctx.HeavyAttackID = 2;
                break;
            case 2:
                Ctx.HeavyAttackID = 3;
                break;
            case 3:
                Ctx.HeavyAttackID = 4;
                break;
            case 4:
                Ctx.HeavyAttackID = 5;
                break;
            case 5:
                Ctx.HeavyAttackID = 1;
                break;
            default:
                break;
        }
    }
}
