using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(StateMachine currentContext, StateFactory StateFactory)
    : base(currentContext, StateFactory) { }

    public override void EnterState()
    {
       // Debug.LogWarning("Player has entered IDLE state");

        Ctx.LightAttackID = 0;
        Ctx.HeavyAttackID = 0;
        Ctx.IsAttacking = false;
        Ctx.OnIdle?.Invoke(true);
        if (!Ctx.IsAI && Ctx.EnemiesNearby.Count == 0)
        {
            Ctx.IsFighting = false;
            Ctx.OnFight?.Invoke(false);
        }
    }

    public override void UpdateState()
    {
        //Debug.Log("IDLE state is currently active");
        Ctx.DebugCurrentSubState = "Idle State";
        CheckSwitchStates();
        
        Ctx.TargetSpeed = 0f;
        SetPlayerMovement();
    }

    public override void ExitState()
    {
        //Debug.LogWarning("Player has exited IDLE state");
        if (!Ctx.IsAI && Ctx.EnemiesNearby.Count == 0)
        {
            Ctx.IsFighting = false;
            Ctx.OnFight?.Invoke(false);
        }
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsDead)
        {
            if (Ctx.MoveInput != Vector2.zero)
            {
                SwitchState(Factory.Move());
            }
            else if (Ctx.IsLightAttackPressed)
            {
                SwitchState(Factory.LightAttack());
            }
            else if (Ctx.IsHeavyAttackPressed)
            {
                SwitchState(Factory.HeavyAttack());
            }
            else if (Ctx.IsLightHitLanded)
            {
                SwitchState(Factory.Hurt());
            }
            else if (Ctx.IsHeavyHitLanded)
            {
                SwitchState(Factory.Hurt());
            }
            else if (Ctx.IsDashPressed)
            {
                SwitchState(Factory.Dash());
            }
            else if (Ctx.IsBlockPressed)
            {
                SwitchState(Factory.Block());
            }
            else if (Ctx.IsParried)
            {
                SwitchState(Factory.Stunned());
            }
        }       
    }

    public override void InitializeSubStates()
    {

    }

    public void SetPlayerMovement()
    {
        if (Ctx.IsFighting || Ctx.EnemiesNearby.Count > 0)
        {
            Ctx.SetCombatMovementAnimationValues();
            Ctx.CombatMovement();
        }
        else
        {
            Ctx.SetFreeRoamMovementAnimationValues();
            Ctx.FreeRoamMovement();
        }
    }
}
