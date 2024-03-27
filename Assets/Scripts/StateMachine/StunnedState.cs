using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunnedState : BaseState
{
    public StunnedState(StateMachine currentContext, StateFactory stateFactory)
      : base(currentContext, stateFactory) { }

    public override void EnterState()
    {
        //SsDebug.LogWarning("Player has entered STUNNED state");
        Ctx.DebugCurrentSubState = "Stunned State";

        Ctx.IsParried = false;
        Ctx.IsKnockedBack = true;
        Ctx.IsStunned = true;
        Ctx.Animator.SetBool(Ctx.AnimIDStunned, true);
        Ctx.Animator.Play($"Stunned", 0, 0);
        ExitAllAnimations();
    }

    public override void UpdateState()
    {
        //Debug.Log("STUNNEDstate is currently active");

        CheckSwitchStates();

        if (Ctx.IsKnockedBack)
        {
            Ctx.SetHitKnockback();
        }
    }

    public override void ExitState()
    {
        //Debug.LogWarning("Player has exited STUNNED state");

        Ctx.Animator.SetBool(Ctx.AnimIDStunned, false);
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsStunned)
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
            Ctx.IsStunned = false;
        }        
    }

    public override void InitializeSubStates()
    {

    }

    public void ExitAllAnimations()
    {
        Ctx.Animator.SetBool(Ctx.AnimIDLightAttack, false);
    }
}
