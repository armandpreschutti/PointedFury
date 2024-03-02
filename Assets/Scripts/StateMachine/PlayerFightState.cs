using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerFightState : PlayerBaseState
{
    public PlayerFightState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
     : base(currentContext, playerStateFactory)
    {
        IsRootState= true;
        InitializeSubStates();
    }

    public override void EnterState()
    {
        //Debug.Log("Player has entered FIGHT state");

        Ctx.OnFight?.Invoke(true);
        Ctx.IsFighting = true;
        Ctx.TargetSpeed = Ctx.FightSpeed;
    }

    public override void UpdateState()
    {
        Debug.Log("FIGHT state is currently active");

        Ctx.FightMovement();
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        //Debug.Log("Player has exited FIGHT state");

        Ctx.OnFight?.Invoke(false);
        Ctx.IsFighting = false;
    }

    public override void CheckSwitchStates()
    {
        if(!Ctx.IsFightPressed)
        {
            SwitchState(Factory.Grounded());
        }
        if (!Ctx.IsGrounded)
        {
            SwitchState(Factory.Fall());
        }
    }

    public override void InitializeSubStates()
    {
        if (Ctx.MoveInput != Vector2.zero)
        {
            SetSubState(Factory.FightIdle());
        }
        else
        {
            SetSubState(Factory.FightStrafe());
        }
    }

   
}
