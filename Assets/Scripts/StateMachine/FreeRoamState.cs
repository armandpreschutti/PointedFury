using UnityEngine;
using DG.Tweening;
using static UnityEngine.Rendering.DebugUI;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class FreeRoamState : BaseState
{
    public FreeRoamState(StateMachine currentContext, StateFactory stateFactory)
    : base(currentContext, stateFactory)
    {
        IsRootState = true;
        InitializeSubStates();
    }

    public override void EnterState()
    {
        //Debug.LogWarning("Player has entered FREE ROAM state");

    }

    public override void UpdateState()
    {
        // Debug.Log("FREE ROAM state is currently active");
        Ctx.DebugCurrentSuperState = "FreeRoam State";
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
        //Debug.LogWarning("Player has exited FREE ROAM state");


    }

    public override void CheckSwitchStates()
    {
        if (Ctx.IsDead)
        {
            SwitchState(Factory.Death());
        }
    }

    public override void InitializeSubStates()
    {   
        if (Ctx.MoveInput != Vector2.zero)
        {
            SetSubState(Factory.Move());
        }
        else
        {
            SetSubState(Factory.Idle());
        }
    }
}
