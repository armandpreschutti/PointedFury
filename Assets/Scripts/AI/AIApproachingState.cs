using UnityEngine;

public class AIApproachingState: AIBaseState
{
    public AIApproachingState(AIBrain currentContext, AIStateFactory stateFactory)
    : base(currentContext, stateFactory) { }

    public override void EnterState()
    {
        //Debug.LogWarning("Enemy has entered APPROACHING state");
    }

    public override void UpdateState()
    {
        // Debug.Log("APPROACHING state is currently active");
        Ctx.DebugSubState = "Approaching State";
        CheckSwitchStates();

        Ctx.timeSinceAttack += Time.deltaTime;
        Ctx.moveInput.y = 1f;
    }

    public override void ExitState()
    {
        //Debug.LogWarning("Enemy has exited APPROACHING state");

    }

    public override void CheckSwitchStates()
    {
        if (Ctx.DistanceToPlayer(Ctx._currentTarget.transform) <= (Ctx.targetDistance))
        {
            SwitchState(Factory.Idle());
        }
    }

    public override void InitializeSubStates()
    {

    }
}
