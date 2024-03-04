using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory) { }

    public override void EnterState()
    {
       Debug.LogWarning("Player has entered RUN state");

       
    }

    public override void UpdateState()
    {
        Debug.Log("RUN state is currently active");
        Ctx.TargetSpeed = Ctx.MoveSpeed;
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Debug.LogWarning("Player has exited RUN state");
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
