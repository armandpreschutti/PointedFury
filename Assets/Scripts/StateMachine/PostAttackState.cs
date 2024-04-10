using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostAttackState : BaseState
{
    public PostAttackState(StateMachine currentContext, StateFactory stateFactory)
    : base(currentContext, stateFactory) { }


    public override void EnterState()
    {
        //Debug.LogWarning("Player has entered POST ATTACK state");
        Ctx.Animator.SetBool(Ctx.AnimIDPostAttack, true);
        Ctx.IsPostAttack = true;
        Ctx.SetAttackDirection();
    }

    public override void UpdateState()
    {
        //Debug.Log("POST ATTACK state is currently active");
        Ctx.DebugCurrentSubState = $"Post Attack State";
        CheckSwitchStates();

        //Ctx.SetAttackDirection();
    }

    public override void ExitState()
    {
        //Debug.LogWarning("Player has exited POST ATTACK state");

        Ctx.IsPostAttack = false;
        Ctx.Animator.SetBool(Ctx.AnimIDPostAttack, false);
    }

    public override void CheckSwitchStates()
    {
        if (Ctx.IsLightAttackPressed)
        {
            SwitchState(Factory.LightAttack());
        }
        else if (Ctx.IsBlockPressed)
        {
            SwitchState(Factory.Block());
        }        
        else if (Ctx.IsHitLanded)
        {
            SwitchState(Factory.Hurt());
        }
        else if (Ctx.IsParrySucces)
        {
            SwitchState(Factory.Parry());
        }
        else if (Ctx.IsDashPressed)
        {
            SwitchState(Factory.Dash());
        }
        else if (!Ctx.IsPostAttack)
        {
            SwitchState(Factory.Idle());
        }
    }

    public override void InitializeSubStates()
    {

    }

  
}
