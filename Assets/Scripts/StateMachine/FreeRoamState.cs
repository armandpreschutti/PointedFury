using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

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
        Ctx.DebugCurrentSuperState = "FreeRoam State";

        Ctx.Animator.SetBool(Ctx.AnimIDGrounded, true);
        //Ctx.IsAttacking = true;
    }

    public override void UpdateState()
    {
        // Debug.Log("FREE ROAM state is currently active");

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

        Ctx.Animator.SetBool(Ctx.AnimIDGrounded, false);

    }

    public override void CheckSwitchStates()
    {
        /*if (Ctx.IsLightAttacking1)
        {
            SwitchState(Factory.Fight());
        }*/
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
