using UnityEngine;

public class AIWatcherRooteState : AIBaseState
{
    public AIWatcherRooteState(AIBrain currentContext, AIStateFactory stateFactory)
    : base(currentContext, stateFactory)
    {
        IsRootState = true;
        InitializeSubStates();
    }

    public override void EnterState()
    {
        //Debug.LogWarning("Player has entered COMBAT state");
        Ctx.targetDistance = Ctx.WatchDistance;
    }

    public override void UpdateState()
    {
        // Debug.Log("COMBAT state is currently active");
        Ctx.DebugSuperState = "Watcher Root State";
        CheckSwitchStates();


    }

    public override void ExitState()
    {
        //Debug.LogWarning("Player has exited COMBAT state");
        Ctx.isWatcher = false;

    }

    public override void CheckSwitchStates()
    {
        if (Ctx.isAttacker)
        {
            SwitchState(Factory.AttackerRootState());
        }
    }

    public override void InitializeSubStates()
    {
        if(Ctx.hitCount > Ctx.HitTolerance)
        {
            SetSubState(Factory.Block());
        }
        else
        {
            SetSubState(Factory.Idle());
        }

    }
}
