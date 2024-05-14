using UnityEditor.Search;
using UnityEngine;

public class AIDisengagingState: AIBaseState
{
    public AIDisengagingState(AIBrain currentContext, AIStateFactory stateFactory)
    : base(currentContext, stateFactory) { }

    public override void EnterState()
    {
        //Debug.LogWarning("Player has entered DISENGAGE state");

        Ctx.comboCount = 0;
    }

    public override void UpdateState()
    {
        // Debug.Log("DISENGAGE state is currently active");
        Ctx.moveInput = Vector2.zero;
        Ctx.DebugSubState = "Disengage State";
        CheckSwitchStates();
        Ctx.moveInput.y = -1f;

    }

    public override void ExitState()
    {
        //Debug.LogWarning("Player has exited Disengage state");


    }

    public override void CheckSwitchStates()
    {
        if (Ctx.DistanceToPlayer(Ctx._currentTarget.transform) >= (Ctx.targetDistance /*+ Ctx.DistanceBuffer*/))
        {
            SwitchState(Factory.Idle());
        }
        /*  if (!Ctx.isHurt)
          {
              SwitchState(Factory.Idle());
          }
          else if (Ctx.hitCount >= Ctx.HitTolerance)
          {
              SwitchState(Factory.Block());
          }*/
    }

    public override void InitializeSubStates()
    {

    }
}
