using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) 
    : base (currentContext, playerStateFactory) { }

    public override void EnterState()
    {
        Debug.LogWarning("Player has entered IDLE state");


    }

    public override void UpdateState()
    {
        Debug.Log("IDLE state is currently active");

        Ctx.TargetSpeed = 0f;
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Debug.LogWarning("Player has exited IDLE state");
    }

    public override void CheckSwitchStates()
    {
        if(Ctx.MoveInput != Vector2.zero)
        {
            if(Ctx.IsSprintPressed && !Ctx.IsFighting)
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
