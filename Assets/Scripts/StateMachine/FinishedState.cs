using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishedState : BaseState
{
    public FinishedState(StateMachine currentContext, StateFactory stateFactory)
      : base(currentContext, stateFactory) { }


    public override void EnterState()
    {
        //Debug.LogWarning("Player has entered POST ATTACK state");

       // Ctx.Controller.transform.position = Ctx.CurrentTarget.GetComponent<StateMachine>().FinishingPosition.position;
       /* Ctx.Controller.enabled = false;
        //Ctx.SetIncomingAttackDirection();
        Ctx.IsFighting = true;
        Ctx.OnFight?.Invoke(true);*/
        Ctx.Animator.Play("Finisher1B", 0, 0);
        Ctx.Animator.SetBool(Ctx.AnimIDFinished, true);
        Ctx.enabled = false;

    }

    public override void UpdateState()
    {
        //Debug.Log("POST ATTACK state is currently active");
        Ctx.DebugCurrentSubState = $"Finished State";
        CheckSwitchStates();
       
    }

    public override void ExitState()
    {
        //Debug.LogWarning("Player has exited POST ATTACK state");

        Ctx.Animator.SetBool(Ctx.AnimIDFinished, false);
    }

    public override void CheckSwitchStates()
    {
        

    }

    public override void InitializeSubStates()
    {

    }
}
