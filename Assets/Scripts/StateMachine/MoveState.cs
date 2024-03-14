using UnityEngine;

public class MoveState : BaseState
{
    public MoveState(StateMachine currentContext, StateFactory stateFactory)
    : base(currentContext, stateFactory) { }

    public override void EnterState()
    {
        //Debug.LogWarning("Player has entered MOVE state");
        Ctx.DebugCurrentSubState = "Run State";

        Ctx.OnRun?.Invoke(true);
    }

    public override void UpdateState()
    {
        //Debug.Log("MOVE state is currently active");

        if (!Ctx.IsFighting)
        {
            Ctx.TargetSpeed = Ctx.MoveSpeed;
        }
        else
        {
            Ctx.TargetSpeed = Ctx.FightSpeed;
        }
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        //Debug.LogWarning("Player has exited MOVE state");

        Ctx.OnRun?.Invoke(false);
    }

    public override void CheckSwitchStates()
    {
        if (Ctx.MoveInput == Vector2.zero)
        {
            SwitchState(Factory.Idle());
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
