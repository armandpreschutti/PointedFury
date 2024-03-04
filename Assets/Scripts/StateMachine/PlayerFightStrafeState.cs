using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFightStrafeState : PlayerBaseState
{
    public PlayerFightStrafeState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
      : base(currentContext, playerStateFactory) { }

    public override void EnterState()
    {
        Debug.LogWarning("Player has entered FIGHT STRAFE state");


    }

    public override void UpdateState()
    {
        Debug.Log("FIGHT STRAFE state is currently active");

        Ctx.TargetSpeed = Ctx.FightSpeed;
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Debug.LogWarning("Player has exited FIGHT STRAFE state");
    }

    public override void CheckSwitchStates()
    {
        if (Ctx.MoveInput == Vector2.zero)
        {
            SwitchState(Factory.FightIdle());
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
