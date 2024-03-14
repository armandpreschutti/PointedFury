using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    public PlayerFallState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
        InitializeSubStates();
    }

    public override void EnterState()
    {
        //Debug.LogWarning("Player has entered FALL state");

        Ctx.OnFall?.Invoke(true);
        Ctx.DebugCurrentSuperState = "Fall State";
    }
    public override void UpdateState()
    {
        //Debug.Log("FALL state is currently active");

        Ctx.VerticalVelocity += Ctx.Gravity * Time.deltaTime;
        Ctx.FreeRoamMovement();
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        //Debug.LogWarning("Player has exited FALL state");

        Ctx.OnFall?.Invoke(false);
    }
    public override void CheckSwitchStates()
    {
        if (Ctx.IsGrounded)
        {
            SwitchState(Factory.Grounded());
        }
    }
    public override void InitializeSubStates()
    {
        if (Ctx.MoveInput != Vector2.zero)
        {
            SetSubState(Factory.Run());
        }
        else
        {
            SetSubState(Factory.Idle());
        }
    }
}
