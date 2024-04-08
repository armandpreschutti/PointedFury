using UnityEngine;

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
        //Debug.LogWarning("Player has entered FIGHT state");
/*        Ctx.IsAttacking = false;
        Ctx.IsPostAttack = false;
        Ctx.Animator.SetBool(Ctx.AnimIDLightAttack, false);
        Ctx.Animator.SetBool(Ctx.AnimIDPostAttack, false);*/
    }

    public override void UpdateState()
    {
        //Debug.Log("FIGHT state is currently active");
        Ctx.DebugCurrentSuperState = "Free RoamState";
        CheckSwitchStates();

        Ctx.SetFreeRoamMovementAnimationValues();
        Ctx.FreeRoamMovement();
        if (Ctx.VerticalVelocity < 0.0f)
        {
            Ctx.VerticalVelocity = -2f;
        }
    }

    public override void ExitState()
    {
        // Debug.LogError("Player has exited FIGHT state");

    }

    public override void CheckSwitchStates()
    {
        if (Ctx.IsFighting)
        {
            SwitchState(Factory.CombatState());
        }
        if (Ctx.IsDead)
        {
            SwitchState(Factory.Death());
        }
    }

    public override void InitializeSubStates()
    {
        if(Ctx.IsAttacking)
        {
            SetSubState(Factory.PostAttack());
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
