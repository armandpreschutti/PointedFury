using UnityEditor.Search;
using UnityEngine;

public class AIIDleState : AIBaseState
{
    public AIIDleState(AIBrain currentContext, AIStateFactory stateFactory)
    : base(currentContext, stateFactory) { }

    public override void EnterState()
    {
        Debug.LogWarning("Player has entered IDLE state");
    }

    public override void UpdateState()
    {
        // Debug.Log("COMBAT state is currently active");
        Ctx.moveInput = Vector2.zero;
        Ctx.DebugSubState = "Idle State";
        CheckSwitchStates();


    }

    public override void ExitState()
    {
        //Debug.LogWarning("Player has exited COMBAT state");


    }

    public override void CheckSwitchStates()
    {
        if(Ctx._isHurt)
        {
            SwitchState(Factory.Hurt());
        }
       /* if (!_isHurt)
        {
            
            if (isAttacker)
            {
                if (TargetDistance(_currentTarget.transform) > (AttackDistance + DistanceBuffer))
                {
                    ChangeState(AIState.Approaching);
                }
                else if (_stateTime > 1f)
                {
                    ChangeState(AIState.Attack);
                }
            }
            else if (isWatcher)
            {
                if (TargetDistance(_currentTarget.transform) > WatchDistance + DistanceBuffer)
                {
                    ChangeState(AIState.Approaching);
                }
                else if (_stateTime > 3f)
                {
                    SetRandomStrafeDirection();
                    ChangeState(AIState.Strafing);
                }
            }
        }
        else
        {
            ChangeState(AIState.Hurt);
        }*/
    }

    public override void InitializeSubStates()
    {
     
    }
}
