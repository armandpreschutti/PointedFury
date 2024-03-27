using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryState : BaseState
{
    public ParryState(StateMachine currentContext, StateFactory stateFactory)
    : base(currentContext, stateFactory) { }


    public override void EnterState()
    {
        Ctx.SetIncomingAttackDirection();
        //Debug.LogWarning("Player has entered PARRY state");
        Ctx.DebugCurrentSubState = $"Parry State";
        Ctx.Animator.SetBool(Ctx.AnimIDParry, true);
        Ctx.Animator.Play($"Parry", 0, 0);
        Ctx.IsParrying = true;
        Ctx.IsParrySucces = false;
    }

    public override void UpdateState()
    {
        //Debug.Log("PARRY state is currently active");

        CheckSwitchStates();


        if (Ctx.IsCharging)
        {
            Ctx.LightAttackMovement();
        }
}

    public override void ExitState()
    {
        //Debug.LogWarning("Player has exited PARRY state");

        Ctx.Animator.SetBool(Ctx.AnimIDParry, false);
        Ctx.IsAttacking = false;
        Ctx.OnLightAttack?.Invoke(false);
        Ctx.IsLightAttacking = false;
        Ctx.IsCharging = false;
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsParrying)
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

    public override void InitializeSubStates()
    {

    }

   
}
