using UnityEngine;

public class MoveState : BaseState
{
    public MoveState(StateMachine currentContext, StateFactory stateFactory)
    : base(currentContext, stateFactory) { }

    public override void EnterState()
    {
        //Debug.LogWarning("Player has entered MOVE state");
        Ctx.DebugCurrentSubState = "Run State";

        Ctx.AttackType = 0;
        Ctx.OnMove?.Invoke(true);

    }

    public override void UpdateState()
    {
        //Debug.Log("MOVE state is currently active");
        CheckSwitchStates();

        Ctx.TargetSpeed = Ctx.FightSpeed;

    }

    public override void ExitState()
    {
        //Debug.LogWarning("Player has exited MOVE state");

        Ctx.OnMove?.Invoke(false);
    }

    public override void CheckSwitchStates()
    {
        if (Ctx.MoveInput == Vector2.zero)
        {
            SwitchState(Factory.Idle());
        }
        if (Ctx.IsLightAttackPressed && !Ctx.IsLightAttacking)
        {
            SwitchState(Factory.LightAttack());
        }
        if (Ctx.IsHitLanded)
        {
            SwitchState(Factory.Hurt());
        }
        if (Ctx.IsDashPressed)
        {
            SwitchState(Factory.Dash());
        }
        if (Ctx.IsDodgeSuccess)
        {
            SwitchState(Factory.Dodge());
        }
    }

    public override void InitializeSubStates()
    {

    }
}
