using UnityEngine;

public class FreeRoamState : BaseState
{
    public FreeRoamState(StateMachine currentContext, StateFactory stateFactory)
    : base(currentContext, stateFactory)
    {
        IsRootState = true;
        InitializeSubStates();
    }

    public override void EnterState()
    {
        //Debug.LogWarning("Player has entered FREE ROAM state");
        Ctx.DebugCurrentSuperState = "FreeRoam State";

        Ctx.OnGrounded?.Invoke(true);
    }

    public override void UpdateState()
    {
        // Debug.Log("FREE ROAM state is currently active");

        CheckSwitchStates();
        Ctx.EnemyDetection();
        Ctx.ThirdPersonMovement();
        if (Ctx.VerticalVelocity < 0.0f)
        {
            Ctx.VerticalVelocity = -2f;
        }
    }

    public override void ExitState()
    {
        //Debug.LogWarning("Player has exited FREE ROAM state");

        Ctx.OnGrounded?.Invoke(false);
    }

    public override void CheckSwitchStates()
    {
        if (Ctx.IsAttacking)
        {
            SwitchState(Factory.Fight());
        }
    }

    public override void InitializeSubStates()
    {
        if (Ctx.MoveInput != Vector2.zero)
        {
            SetSubState(Factory.Move());
        }
        else
        {
            SetSubState(Factory.Idle());
        }
    }
}
