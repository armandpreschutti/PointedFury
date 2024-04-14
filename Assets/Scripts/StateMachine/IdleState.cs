using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(StateMachine currentContext, StateFactory StateFactory)
    : base(currentContext, StateFactory) { }

    public override void EnterState()
    {
       // Debug.LogWarning("Player has entered IDLE state");

        Ctx.AttackID = 0;
        Ctx.IsAttacking = false;
        Ctx.OnIdle?.Invoke(true);
        if (!Ctx.IsAI)
        {
            Ctx.IsFighting = false;
        }
    }

    public override void UpdateState()
    {
        //Debug.Log("IDLE state is currently active");
        Ctx.DebugCurrentSubState = "Idle State";
        CheckSwitchStates();
        
        Ctx.TargetSpeed = 0f;

    }

    public override void ExitState()
    {
        //Debug.LogWarning("Player has exited IDLE state");

        Ctx.OnIdle?.Invoke(false);
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsDead)
        {
            if (Ctx.MoveInput != Vector2.zero)
            {
                SwitchState(Factory.Move());
            }
            if (Ctx.IsLightAttackPressed/* && !Ctx.IsHurt*/)
            {
                SwitchState(Factory.LightAttack());
            }
            if (Ctx.IsHeavyAttackPressed /*&& !Ctx.IsHurt*/)
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
    }

    public override void InitializeSubStates()
    {

    }
}
