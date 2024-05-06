using UnityEditor.Search;
using UnityEngine;

public class AIIDleState : AIBaseState
{
    public AIIDleState(AIBrain currentContext, AIStateFactory stateFactory)
    : base(currentContext, stateFactory) { }


    float stateTime;
    float attackTime = Random.Range(0f, 2f);
    public override void EnterState()
    {
        //Debug.LogWarning("Player has entered IDLE state");

        Ctx.comboCount = 0;
    }

    public override void UpdateState()
    {
        // Debug.Log("COMBAT state is currently active");
        Ctx.DebugSubState = "Idle State";
        CheckSwitchStates();

        stateTime += Time.deltaTime;
        Ctx.moveInput = Vector2.zero;
    }

    public override void ExitState()
    {
        //Debug.LogWarning("Player has exited COMBAT state");


    }

    public override void CheckSwitchStates()
    {

        if (Ctx.DistanceToPlayer(Ctx._currentTarget.transform) > (Ctx.targetDistance + Ctx.DistanceBuffer))
        {
            SwitchState(Factory.Approaching());
        }
        else if (Ctx.isAttacker && stateTime > attackTime)
        {
            SwitchState(Factory.Attack());
        }
        else if (Ctx.isWatcher && stateTime > 3f)
        {
            SwitchState(Factory.Strafing());
        }
        else if (Ctx.isHurt)
        {
            SwitchState(Factory.Hurt());
        }
    }

    public override void InitializeSubStates()
    {
     
    }
}
