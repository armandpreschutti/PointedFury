using UnityEditor.Search;
using UnityEngine;

public class AIHurtState : AIBaseState
{
    public AIHurtState(AIBrain currentContext, AIStateFactory stateFactory)
    : base(currentContext, stateFactory) { }

    public override void EnterState()
    {
        //Debug.LogWarning("Player has entered HURT state");
    }

    public override void UpdateState()
    {
        // Debug.Log("COMBAT state is currently active");
        Ctx.moveInput = Vector2.zero;
        Ctx.DebugSubState = "Hurt State";
        CheckSwitchStates();


    }

    public override void ExitState()
    {
        //Debug.LogWarning("Player has exited COMBAT state");


    }

    public override void CheckSwitchStates()
    {
        if(!Ctx._isHurt)
        {
            SwitchState(Factory.Idle());
        }
    }

    public override void InitializeSubStates()
    {

    }
}
