using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeState : BaseState
{
    public EvadeState(StateMachine currentContext, StateFactory stateFactory)
     : base(currentContext, stateFactory) { }


    public override void EnterState()
    {
        //Debug.LogWarning("Player has entered DASH state");

        if (Ctx.CurrentTarget != null && Ctx.CurrentTarget.GetComponent<StateMachine>().IsEvadable)
        {
            Ctx.Animator.SetBool(Ctx.AnimIDEvade, true);
            Ctx.IsEvading = true;
        }
        else
        {
            Ctx.SetDashDirection();
            Ctx.Animator.SetBool(Ctx.AnimIDDash, true);

        }
        // Ctx.Animator.Play($"Dash", 0, 0);
        //Ctx.Animator.SetBool(Ctx.AnimIDDash, true);
        Ctx.IsDashing = true;
        Ctx.IsFighting = true;
        Ctx.IsDashPressed = false;
        Ctx.OnDashSuccessful?.Invoke();
    }

    public override void UpdateState()
    {
        //Debug.Log("DASH state is currently active");
        Ctx.DebugCurrentSubState = "Dash State";
        CheckSwitchStates();

        if (Ctx.IsDashMoving)
        {

            Ctx.DashMovement();
        }
    }

    public override void ExitState()
    {
        //Debug.LogWarning("Player has exited state");

        Ctx.Animator.SetBool(Ctx.AnimIDDash, false);
        Ctx.Animator.SetBool(Ctx.AnimIDEvade, false);
        Ctx.IsDashing = false;
        Ctx.IsEvading = false;
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsDashing)
        {
            if (Ctx.IsBlockPressed)
            {
                SwitchState(Factory.Block());
            }
            else if (Ctx.IsLightAttackPressed)
            {
                SwitchState(Factory.LightAttack());
            }
            else
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
        }
    }

    public override void InitializeSubStates()
    {

    }
}
