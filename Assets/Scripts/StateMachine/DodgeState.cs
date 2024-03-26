using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeState : BaseState
{
    public DodgeState(StateMachine currentContext, StateFactory stateFactory)
       : base(currentContext, stateFactory) { }

    public override void EnterState()
    {
        //Debug.LogWarning("Player has entered DODGE state");

        Ctx.IsDodgeSuccess = false;
        Ctx.IsDodging = true;
        Ctx.Animator.SetBool(Ctx.AnimIDDodge, true);
        Ctx.Animator.Play($"Dodge{Ctx.DodgeType()}", 0, 0);
    }

    public override void UpdateState()
    {
        //Debug.Log("DODGE state is currently active");

        Ctx.DebugCurrentSubState = "Dodge State";
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        //Debug.LogWarning("Player has exited DODGE state");

    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsDodging)
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
        if(Ctx.IsDodgeSuccess)
        {
            SwitchState(Factory.Dodge());
        }
    }

    public override void InitializeSubStates()
    {

    }
}
