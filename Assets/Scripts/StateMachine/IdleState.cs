using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(StateMachine currentContext, StateFactory StateFactory)
    : base(currentContext, StateFactory) { }

    public override void EnterState()
    {
       // Debug.LogWarning("Player has entered IDLE state");
        Ctx.DebugCurrentSubState = "Idle State";

        Ctx.AttackType = 0;
        Ctx.HitType = 0;
        Ctx.OnIdle?.Invoke(true);
    }

    public override void UpdateState()
    {
        //Debug.Log("IDLE state is currently active");
        CheckSwitchStates();
        Ctx.TargetSpeed = 0f;

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
        if (Ctx.IsLightAttackPressed)
        {
            SwitchState(Factory.LightAttack1());
        }
        if (Ctx.HitLanded)
        {
            SwitchState(Factory.Hurt());
        }
    }

    public override void InitializeSubStates()
    {

    }
}
