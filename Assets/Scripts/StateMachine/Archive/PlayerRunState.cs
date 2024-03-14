using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory) { }

    public override void EnterState()
    {
       Debug.LogWarning("Player has entered RUN state");

        Ctx.DebugCurrentSubState = "Run State";
        Ctx.OnRun?.Invoke(true);
    }

    public override void UpdateState()
    {
        //Debug.Log("RUN state is currently active");

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
        //Debug.LogWarning("Player has exited RUN state");

        Ctx.OnRun?.Invoke(false);
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
        if (Ctx.IsLightAttackPressed && !Ctx.IsAttacking)
        {
            SwitchState(Factory.LightAttack());
        }
    }

    public override void InitializeSubStates()
    {

    }
}
