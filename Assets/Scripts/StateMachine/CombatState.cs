using UnityEngine;
using DG.Tweening;
using static UnityEngine.Rendering.DebugUI;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class CombatState : BaseState
{
    public CombatState(StateMachine currentContext, StateFactory stateFactory)
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
        Ctx.DebugCurrentSuperState = "Combat State";
        CheckSwitchStates();
        if (Ctx.IsFighting)
        {
            Ctx.SetCombatMovementAnimationValues();
            Ctx.CombatMovement();
        }
        else
        {
            Ctx.SetFreeRoamMovementAnimationValues();
            Ctx.FreeRoamMovement();
        }


        if (Ctx.VerticalVelocity < 0.0f)
        {
            Ctx.VerticalVelocity = -2f;
        }
    }

    public override void ExitState()
    {
        //Debug.LogWarning("Player has exited COMBAT state");


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
        if(Ctx.IsAttacking)
        {
            SetSubState(Factory.LightAttack());
        }
        else
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
}
