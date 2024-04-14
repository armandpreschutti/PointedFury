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
        Ctx.Animator.Play($"HeavyAttack{Ctx.AttackID}", 0, 0);
        Ctx.Animator.SetBool(Ctx.AnimIDHeavyAttack, true);
        Ctx.IsAttacking = true;
        Ctx.IsHeavyAttackPressed = false;
        Ctx.IsFighting = true;
        Ctx.OnHeavyAttack?.Invoke(true);
        Ctx.IsBlockPressed = false;
    }

    public override void UpdateState()
    {
        //Debug.Log("LIGHT ATTACK state is currently active");
        Ctx.DebugCurrentSubState = $"Heavy Attack {Ctx.AttackID} State";
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
        Ctx.OnHeavyAttack?.Invoke(false);
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
        if (Ctx.IsLightHitLanded)
        {
            SwitchState(Factory.Hurt());
        }
        if (Ctx.IsHeavyHitLanded)
        {
            SwitchState(Factory.Stunned());
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
        Ctx.AttackType = "Heavy";
        switch (Ctx.AttackID)
        {
            case 0:
                Ctx.AttackID = 1;
                break;
            case 1:
                Ctx.AttackID = 2;
                break;
            case 2:
                Ctx.AttackID = 3;
                break;
            case 3:
                Ctx.AttackID = 4;
                break;
            case 4:
                Ctx.AttackID = 5;
                break;
            case 5:
                Ctx.AttackID = 6;
                break;
            case 6:
                Ctx.AttackID = 7;
                break;
            case 7:
                Ctx.AttackID = 1;
                break;
            default:
                break;
        }
    }
}
