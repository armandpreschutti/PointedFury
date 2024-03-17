using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAttack7State : BaseState
{
    public LightAttack7State(StateMachine currentContext, StateFactory stateFactory)
     : base(currentContext, stateFactory) { }


    public override void EnterState()
    {
        Debug.LogWarning("Player has entered LIGHT ATTACK 3 state");
        Ctx.DebugCurrentSubState = "Light Attack 3 State";

        Ctx.AttackType = 7;
        Ctx.IsLightAttacking7 = true;
        Ctx.IsAttacking = true;
        Ctx.IsLightAttackPressed = false;
        Ctx.IsComboAttacking = false;
        Ctx.IsFighting = true;
    }

    public override void UpdateState()
    {
        //Debug.Log("LIGHT ATTACK state is currently active");
        CheckSwitchStates();

        Ctx.RotateTowardTarget(.01f);
        Ctx.TargetSpeed = 0f;

    }

    public override void ExitState()
    {
        //Debug.Log("Player has exited LIGHT ATTACK state");

        Ctx.Animator.SetBool(Ctx.AnimIDLightAttack7, false);
        Ctx.IsAttacking = false;
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsLightAttacking7)
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

    public override void InitializeSubStates()
    {

    }
}
