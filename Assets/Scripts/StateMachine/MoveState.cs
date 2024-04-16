using UnityEngine;

public class MoveState : BaseState
{
    public MoveState(StateMachine currentContext, StateFactory stateFactory)
    : base(currentContext, stateFactory) { }

    public override void EnterState()
    {
        //Debug.LogWarning("Player has entered MOVE state");

        Ctx.LightAttackID = 0;
        Ctx.HeavyAttackID = 0;
        Ctx.IsAttacking = false;
        Ctx.OnMove?.Invoke(true);
        if (!Ctx.IsAI)
        {
            Ctx.IsFighting = false;
        }
        
    }

    public override void UpdateState()
    {
        //Debug.Log("MOVE state is currently active");
        Ctx.DebugCurrentSubState = "Move State";
        CheckSwitchStates();

        if (Ctx.IsFighting)
        {
            Ctx.TargetSpeed = Ctx.FightSpeed;
        }
        else
        {
            Ctx.TargetSpeed = Ctx.MoveSpeed;
        }


    }

    public override void ExitState()
    {
        //Debug.LogWarning("Player has exited MOVE state");

        Ctx.OnMove?.Invoke(false);
    }

    public override void CheckSwitchStates()
    {
        if (Ctx.MoveInput == Vector2.zero)
        {
            SwitchState(Factory.Idle());
        }
        if (Ctx.IsLightAttackPressed && !Ctx.IsAttacking)
        {
            SwitchState(Factory.LightAttack());
        }
        if(Ctx.IsHeavyAttackPressed&& !Ctx.IsAttacking)
        {
            SwitchState(Factory.HeavyAttack());
        }
        if (Ctx.IsLightHitLanded)
        {
            SwitchState(Factory.Hurt());
        }
        if (Ctx.IsHeavyHitLanded)
        {
            SwitchState(Factory.Stunned());
        }
        if (Ctx.IsDashPressed)
        {
            SwitchState(Factory.Dash());
        }
        if (Ctx.IsBlockPressed)
        {
            SwitchState(Factory.Block());
        }
        if (Ctx.IsParrySucces)
        {
            SwitchState(Factory.Parry());
        }
        if (Ctx.IsParried)
        {
            SwitchState(Factory.Stunned());
        }
    }

    public override void InitializeSubStates()
    {

    }
}
