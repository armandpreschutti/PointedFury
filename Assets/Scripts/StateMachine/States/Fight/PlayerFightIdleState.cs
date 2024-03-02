using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFightIdleState : PlayerBaseState
{
    public PlayerFightIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory) { }

    public override void EnterState()
    {
        //Debug.Log("Player has entered FIGHT IDLE state");

        Ctx.TargetSpeed = 0f;
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
       
    }

    public override void InitializeSubStates()
    {

    }
}
