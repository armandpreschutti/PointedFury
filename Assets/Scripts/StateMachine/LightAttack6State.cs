using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAttack6State : BaseState
{
    public LightAttack6State(StateMachine currentContext, StateFactory stateFactory)
        : base(currentContext, stateFactory) { }


    public override void EnterState()
    {
        Debug.LogWarning("Player has entered LIGHT ATTACK 2 state");
        Ctx.DebugCurrentSubState = "Light Attack 2 State";

        //Ctx.IsFighting = true;
        Ctx.AttackType = 6;
        Ctx.IsLightAttacking6 = true;
        Ctx.IsAttacking = true;
        Ctx.IsLightAttackPressed = false;
        Ctx.IsComboAttacking = false;
        Ctx.CanComboAttack = true;
        Ctx.IsFighting = true;
    }

    public override void UpdateState()
    {
        CheckSwitchStates();

        Ctx.RotateTowardTarget();
        Ctx.TargetSpeed = 0f;
        if (Ctx.IsCharging)
        {
            Ctx.ChargeAtEnemy();
        }
        if (Ctx.IsLightAttackPressed)
        {
            Ctx.IsComboAttacking = true;
            Ctx.CanComboAttack = false;
            Ctx.Animator.SetBool(Ctx.AnimIDLightAttack7, true);
        }
    }

    public override void ExitState()
    {
        Debug.Log("Player has exited LIGHT ATTACK 2 state");

        //Ctx.OnLightAttack2?.Invoke(true);
        Ctx.Animator.SetBool(Ctx.AnimIDLightAttack6, false);
        Ctx.IsAttacking = false;
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsLightAttacking6)
        {
            if (Ctx.IsComboAttacking)
            {
                SwitchState(Factory.LightAttack7());
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
    }

    public override void InitializeSubStates()
    {

    }
}
