using UnityEngine;

public class AIAttackerRooteState: AIBaseState
{
    public AIAttackerRooteState(AIBrain currentContext, AIStateFactory stateFactory)
    : base(currentContext, stateFactory)
    {
        IsRootState = true;
        InitializeSubStates();
    }

    public override void EnterState()
    {
        //Debug.LogWarning("Player has entered COMBAT state");
    }

    public override void UpdateState()
    {
        // Debug.Log("COMBAT state is currently active");
        Ctx.DebugSuperState = "Attacker Root State";

        CheckSwitchStates();


    }

    public override void ExitState()
    {
        //Debug.LogWarning("Player has exited COMBAT state");
        Ctx.isAttacker = false;

    }

    public override void CheckSwitchStates()
    {
        if (Ctx.isWatcher)
        {
            SwitchState(Factory.WatcherRootState());
        }
    }

    public override void InitializeSubStates()
    {
        SetSubState(Factory.Idle());
    }
}
