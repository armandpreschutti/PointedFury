using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(StateMachine currentContext, StateFactory StateFactory)
    : base(currentContext, StateFactory) { }

    public override void EnterState()
    {
       // Debug.LogWarning("Player has entered IDLE state");

        Ctx.AttackType = 0;

        Ctx.OnIdle?.Invoke(true);
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
            if (Ctx.IsLightAttackPressed && !Ctx.IsHurt)
            {
                SwitchState(Factory.LightAttack());
            }
            if (Ctx.IsHitLanded)
            {
                SwitchState(Factory.Hurt());
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
