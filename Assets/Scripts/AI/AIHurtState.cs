using UnityEditor.Search;
using UnityEngine;

public class AIHurtState : AIBaseState
{
    public AIHurtState(AIBrain currentContext, AIStateFactory stateFactory)
    : base(currentContext, stateFactory) { }

    public override void EnterState()
    {
        //Debug.LogWarning("Player has entered HURT state");

        Ctx.comboCount = 0;
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
        if(!Ctx.isHurt)
        {
            SwitchState(Factory.Idle());
        }
        else if(Ctx.hitCount >= Ctx.HitTolerance)
        {
            SwitchState(Factory.Block());
        }
    }

    public override void InitializeSubStates()
    {

    }
}
