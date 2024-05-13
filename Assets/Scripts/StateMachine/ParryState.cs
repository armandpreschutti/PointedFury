using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ParryState : BaseState
{
    public ParryState(StateMachine currentContext, StateFactory stateFactory)
    : base(currentContext, stateFactory) { }


    public override void EnterState()
    {
        //Debug.LogWarning("Player has entered PARRY state");

        Ctx.IsParrying = true;
        Ctx.IsAttacking = false;
        Ctx.Animator.SetBool(Ctx.AnimIDParry, true);
        //Ctx.Animator.Play($"Parry", 0, 0);
        Ctx.IsFighting = true;
        Ctx.OnFight?.Invoke(true);
        Ctx.IsParrySucces = false;
    }

    public override void UpdateState()
    {
        //Debug.Log("PARRY state is currently active");
        Ctx.DebugCurrentSubState = $"Parry State";
        CheckSwitchStates();

        if(Ctx.IsCharging)
        {
            Ctx.SetIncomingAttackDirection();
            Ctx.ParryMovement();
        }

    }

    public override void ExitState()
    {
        //Debug.LogWarning("Player has exited PARRY state");
        Time.timeScale = 1f;
        Ctx.Animator.SetBool(Ctx.AnimIDParry, false);
        Ctx.IsParrying = false;
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsParrying)
        {
            if (Ctx.IsBlockPressed)
            {
                SwitchState(Factory.Block());
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
