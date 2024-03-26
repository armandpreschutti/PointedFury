using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : BaseState
{
    public DashState(StateMachine currentContext, StateFactory stateFactory)
     : base(currentContext, stateFactory) { }


    public override void EnterState()
    {
        //Debug.LogWarning("Player has entered DASH state");
        Ctx.DebugCurrentSubState = "Dash State";

        Ctx.Animator.SetBool(Ctx.AnimIDDash, true);
        Ctx.IsDashing = true;
        Ctx.IsDashPressed= false;
        Ctx.IsFighting = true;
    }

    public override void UpdateState()
    {
        //Debug.Log("DASH state is currently active");

        CheckSwitchStates();

        //Ctx.SetAttackDirection();
        if (Ctx.IsDashMoving)
        {
            Ctx.DashMovement();
        }
       
    }

    public override void ExitState()
    {
        //Debug.LogWarning("Player has exited state");

        // Ctx.IsLightAttacking1 = false;
        Ctx.Animator.SetBool(Ctx.AnimIDDash, false);
        //Ctx.IsDodging = false;

    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsDashing)
        {
            if (Ctx.MoveInput != Vector2.zero)
            {
                SwitchState(Factory.Move());
            }
            else
            {
                SwitchState(Factory.Idle());
            }

        }
        if (Ctx.IsHitLanded)
        {
            SwitchState(Factory.Hurt());
            Ctx.IsDashing= false;
        }
    }

    public override void InitializeSubStates()
    {

    }
}
