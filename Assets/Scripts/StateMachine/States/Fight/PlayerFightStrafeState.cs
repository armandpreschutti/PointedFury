using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFightStrafeState : PlayerBaseState
{
    public PlayerFightStrafeState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
      : base(currentContext, playerStateFactory) { }

    public override void EnterState()
    {
        //Debug.Log("Player has entered FIGHT IDLE state");

        Ctx.TargetSpeed = Ctx.FightSpeed;
    }

    public override void UpdateState()
    {
        Debug.Log("FIGHT IDLE state is currently active");

        CheckSwitchStates();
    }

    public override void ExitState()
    {
        //Debug.Log("Player has exited FIGHT IDLE state");
    }

    public override void CheckSwitchStates()
    {
        if (Ctx.MoveInput != Vector2.zero)
        {
            if (Ctx.IsSprintPressed && !Ctx.IsFighting)
            {
                SwitchState(Factory.Sprint());
            }
            else
            {
                SwitchState(Factory.Run());
            }
        }
    }

    public override void InitializeSubStates()
    {

    }
}
