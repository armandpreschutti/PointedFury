using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) 
    : base (currentContext, playerStateFactory) { }

    public override void EnterState()
    {
        //Debug.Log("Player has entered IDLE state");

        Ctx.TargetSpeed = 0f;
    }

    public override void UpdateState()
    {
        Debug.Log("IDLE state is currently active");

        CheckSwitchStates();
    }

    public override void ExitState()
    {
        //Debug.Log("Player has exited IDLE state");
    }

    public override void CheckSwitchStates()
    {
        if(Ctx.MoveInput != Vector2.zero)
        {
            if(Ctx.IsSprintPressed)
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
