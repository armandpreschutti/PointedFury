using UnityEngine;

public class FightState : BaseState
{
    public FightState(StateMachine currentContext, StateFactory stateFactory)
     : base(currentContext, stateFactory)
    {
        IsRootState = true;
        InitializeSubStates();
    }

    public override void EnterState()
    {
        //Debug.LogWarning("Player has entered FIGHT state");
        Ctx.DebugCurrentSuperState = "Fight State";

        Ctx.OnFight?.Invoke(true);
        Ctx.IsFighting = true;

    }

    public override void UpdateState()
    {
        //Debug.Log("FIGHT state is currently active");

        SetFightTimeout();
        CheckSwitchStates();
        Ctx.EnemyDetection();
        Ctx.FightMovement();
        if (Ctx.VerticalVelocity < 0.0f)
        {
            Ctx.VerticalVelocity = -2f;
        }
    }

    public override void ExitState()
    {
        // Debug.LogError("Player has exited FIGHT state");

        Ctx.OnFight?.Invoke(false);
        Ctx.IsFighting = false;
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsFighting)
        {
            SwitchState(Factory.FreeRoam());
        }
    }

    public override void InitializeSubStates()
    {
        if(!Ctx.IsFighting)
        {
            SwitchState(Factory.FreeRoam());
        }
    }
    public void SetFightTimeout()
    {
        if (Ctx.FightTimeoutActive && Ctx.FightTimeoutDelta > 0f)
        {
            Ctx.FightTimeoutDelta -= Time.deltaTime;

        }
        else
        {
            Ctx.FightTimeoutDelta = 0f;
            Ctx.IsFighting = false;
        }
    }

}
