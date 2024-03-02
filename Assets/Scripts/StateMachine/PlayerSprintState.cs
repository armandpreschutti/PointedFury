using UnityEngine;

public class PlayerSprintState : PlayerBaseState
{
    public PlayerSprintState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory) { }

    public override void EnterState()
    {
       // Debug.Log("Player has entered SPRINT state");

        Ctx.TargetSpeed = Ctx.SprintSpeed;
    }

    public override void UpdateState()
    {
        Debug.Log("SPRINT state is currently active");
        CheckSwitchStates();
    }

    public override void ExitState()
    {
       // Debug.Log("Player has exited SPRINT state");
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsSprintPressed || !Ctx.IsGrounded || Ctx.MoveInput == Vector2.zero)
        {
            if(Ctx.MoveInput != Vector2.zero)
            {
                SwitchState(Factory.Run());
            }
            else
            {
                SwitchState(Factory.Idle());
            }
        }
    }

    public override void InitializeSubStates()
    {

    }
}
