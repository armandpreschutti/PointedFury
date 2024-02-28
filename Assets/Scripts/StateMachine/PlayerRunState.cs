using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory) { }

    public override void EnterState()
    {
        //Debug.Log("Player has entered RUN state");

        Ctx.TargetSpeed = Ctx.MoveSpeed;
    }

    public override void UpdateState()
    {
        Debug.Log("RUN state is currently active");

        CheckSwitchStates();
    }

    public override void ExitState()
    {
        //Debug.Log("Player has exited RUN state");
    }

    public override void CheckSwitchStates()
    {
        if(Ctx.MoveInput == Vector2.zero)
        {
            SwitchState(Factory.Idle());
        }
        if(Ctx.IsSprintPressed && Ctx.IsGrounded)
        {
            SwitchState(Factory.Sprint());
        }
       
    }

    public override void InitializeSubStates()
    {

    }
}
