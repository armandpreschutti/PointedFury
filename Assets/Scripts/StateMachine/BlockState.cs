using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class BlockState : BaseState
{
    public BlockState(StateMachine currentContext, StateFactory stateFactory)
       : base(currentContext, stateFactory) { }

    public override void EnterState()
    {
        //Debug.LogWarning("Player has entered BLOCK state");

        Ctx.SetAttackDirection();
        Ctx.AttackType = 0;
        Ctx.IsBlockSuccess = false;
        Ctx.IsBlocking = true;
        Ctx.Animator.SetBool(Ctx.AnimIDBlock, true);
    }

    public override void UpdateState()
    {
        //Debug.Log("BLOCK state is currently active");
        Ctx.DebugCurrentSubState = "Block State";
        CheckSwitchStates();

        if (Ctx.IsBlockSuccess)
        {
            Ctx.SetIncomingAttackDirection();
            Ctx.Animator.Play($"BlockImpact", 0, 0);
            Ctx.IsBlockSuccess = false;
        }
    }

    public override void ExitState()
    {
        //Debug.LogWarning("Player has exited BLOCK state");

        Ctx.Animator.SetBool(Ctx.AnimIDBlock, false);
        Ctx.IsBlocking = false;
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsBlockPressed)
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
     /*   if (Ctx.IsLightAttackPressed)
        {
            SwitchState(Factory.LightAttack());
        }*/
        if (Ctx.IsDashPressed)
        {
            SwitchState(Factory.Dash());
        }
        if (Ctx.IsParrySucces)
        {
            SwitchState(Factory.Parry());
        }
    }

    public override void InitializeSubStates()
    {

    }
}
