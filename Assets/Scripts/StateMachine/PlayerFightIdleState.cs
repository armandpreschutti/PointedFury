using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFightIdleState : PlayerBaseState
{
    public PlayerFightIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory) { }

    public override void EnterState()
    {
        Debug.LogWarning("Player has entered FIGHT IDLE state");

    }

    public override void UpdateState()
    {
        Debug.Log("FIGHT IDLE state is currently active");

        Ctx.TargetSpeed = 0f;
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Debug.LogWarning("Player has exited FIGHT IDLE state");
    }

    public override void CheckSwitchStates()
    {
        if (Ctx.MoveInput != Vector2.zero)
        {
            SwitchState(Factory.FightStrafe());
        }
        if (Ctx.IsLightAttackPressed && !Ctx.IsAttacking)
        {
            SwitchState(Factory.LightAttack());
        }
    }

    public override void InitializeSubStates()
    {

    }
}
