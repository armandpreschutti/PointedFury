using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;

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
        Debug.LogWarning("Player has entered FIGHT state");

        Ctx.OnFight?.Invoke(true);
        Ctx.IsFighting = true;

    }

    public override void UpdateState()
    {
        Debug.Log("FIGHT state is currently active");

        Ctx.FreeMovement();
        Ctx.EnemyDetection();
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Debug.LogError("Player has exited FIGHT state");
        Ctx.attackSequence.Pause();
        Ctx.OnFight?.Invoke(false);
        Ctx.IsFighting = false;
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsFightPressed && !Ctx.IsAttacking)
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
            SetSubState(Factory.FightStrafe());
        }
        else
        {
            SetSubState(Factory.FightIdle());
        }
    }

   
}
