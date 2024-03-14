using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(StateMachine currentContext, StateFactory StateFactory)
    : base(currentContext, StateFactory) { }

    public override void EnterState()
    {
        Debug.LogWarning("Player has entered IDLE state");
        Ctx.DebugCurrentSubState = "Idle State";

        Ctx.OnIdle?.Invoke(true);
    }

    public override void UpdateState()
    {
        //Debug.Log("IDLE state is currently active");

        Ctx.TargetSpeed = 0f;
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        //Debug.LogWarning("Player has exited IDLE state");

        Ctx.OnIdle?.Invoke(false);
    }

    public override void CheckSwitchStates()
    {
        if (Ctx.MoveInput != Vector2.zero)
        {
            SwitchState(Factory.Move());
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
