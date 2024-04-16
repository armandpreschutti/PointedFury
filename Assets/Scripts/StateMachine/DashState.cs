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
        Ctx.SetDashDirection();
        Ctx.Animator.SetBool(Ctx.AnimIDDash, true);
        Ctx.Animator.Play($"Dash", 0, 0);
        Ctx.IsDashing = true;
        Ctx.IsFighting = true;
        Ctx.IsDashPressed= false;
        Ctx.OnDashSuccessful?.Invoke();        
    }

    public override void UpdateState()
    {
        //Debug.Log("DASH state is currently active");
        Ctx.DebugCurrentSubState = "Dash State";
        CheckSwitchStates();

        Ctx.LightAttackID = 0;
        Ctx.HeavyAttackID = 0;
        if (Ctx.IsDashMoving)
        {

            Ctx.DashMovement();
        }       
    }

    public override void ExitState()
    {
        //Debug.LogWarning("Player has exited state");

        Ctx.Animator.SetBool(Ctx.AnimIDDash, false);
        Ctx.IsDashing = false;
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsDashing)
        {
            if (Ctx.MoveInput != Vector2.zero)
            {
                SwitchState(Factory.Move());
            }
            else if (Ctx.IsBlockPressed)
            {
                SwitchState(Factory.Block());
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
