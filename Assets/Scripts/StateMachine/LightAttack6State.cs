using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAttack6State : BaseState
{
    public LightAttack6State(StateMachine currentContext, StateFactory stateFactory)
        : base(currentContext, stateFactory) { }


    public override void EnterState()
    {
        //Debug.LogWarning("Player has entered LIGHT ATTACK 6 state");
        Ctx.DebugCurrentSubState = "Light Attack 6 State";

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
        //Debug.Log("LIGHT ATTACK 6 state is currently active");
        CheckSwitchStates();

        Ctx.SetAttackDirection();
/*        Ctx.TargetSpeed = 0f;*/
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
        //Debug.Log("Player has exited LIGHT ATTACK 6 state");

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
