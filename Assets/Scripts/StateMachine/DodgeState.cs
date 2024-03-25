using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeState : BaseState
{
    public DodgeState(StateMachine currentContext, StateFactory stateFactory)
     : base(currentContext, stateFactory) { }

    Vector3 dodgeDirection;
    public override void EnterState()
    {
        //Debug.LogWarning("Player has entered LIGHT ATTACK 1 state");
        Ctx.DebugCurrentSubState = "Dodge State";

        Ctx.Animator.SetBool(Ctx.AnimIDDodge, true);
        Ctx.IsDodging = true;
        Ctx.IsDodgePressed= false;
        Ctx.IsFighting = true;
        dodgeDirection = Ctx.InputDirection();

    }

    public override void UpdateState()
    {
        //Debug.Log("LIGHT ATTACK state is currently active");

        CheckSwitchStates();

        ///Ctx.SetAttackDirection();
        if (Ctx.IsLeaping)
        {
            Ctx.DodgeMovement(dodgeDirection);
        }
       
    }

    public override void ExitState()
    {
        //Debug.LogWarning("Player has exited LIGHT ATTACK state");

        // Ctx.IsLightAttacking1 = false;
        Ctx.Animator.SetBool(Ctx.AnimIDDodge, false);
        //Ctx.IsDodging = false;

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
        if (Ctx.IsHitLanded)
        {
            SwitchState(Factory.Hurt());
            Ctx.IsDodging= false;
        }
    }

    public override void InitializeSubStates()
    {

    }
}
