using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishingState : BaseState
{
    public FinishingState(StateMachine currentContext, StateFactory stateFactory)
     : base(currentContext, stateFactory) { }


    public override void EnterState()
    {
        //Debug.LogWarning("Player has entered POST ATTACK state");

        Ctx.Controller.enabled = false;

        Ctx.IsFighting = true;
        Ctx.OnFight?.Invoke(true);
        Ctx.Animator.Play("Finisher1A", 0, 0);
        Ctx.Animator.SetBool(Ctx.AnimIDFinishing, true);
        //Ctx.SetAttackDirection();
    }

    public override void UpdateState()
    {
        //Debug.Log("POST ATTACK state is currently active");
        Ctx.DebugCurrentSubState = $"Finishing State";
       
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        //Debug.LogWarning("Player has exited POST ATTACK state");
        Ctx.Controller.enabled = true;
        Ctx.Animator.SetBool(Ctx.AnimIDFinishing, false);
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsFinishing)
        {

            SwitchState(Factory.Idle());
        }

    }

    public override void InitializeSubStates()
    {

    }
}
