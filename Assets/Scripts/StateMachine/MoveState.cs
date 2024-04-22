using UnityEngine;

public class MoveState : BaseState
{
    public MoveState(StateMachine currentContext, StateFactory stateFactory)
    : base(currentContext, stateFactory) { }

    public override void EnterState()
    {
        //Debug.LogWarning("Player has entered MOVE state");

/*        Ctx.LightAttackID = 0;
        Ctx.HeavyAttackID = 0;*/
        Ctx.IsAttacking = false;
        Ctx.OnMove?.Invoke(true);
        if (!Ctx.IsAI && Ctx.EnemiesNearby.Count == 0)
        {
            Ctx.IsFighting = false;
            Ctx.OnFight?.Invoke(false);
        }

    }

    public override void UpdateState()
    {
        //Debug.Log("MOVE state is currently active");
        Ctx.DebugCurrentSubState = "Move State";
        CheckSwitchStates();

        if (Ctx.IsFighting || Ctx.EnemiesNearby.Count > 0)
        {
            Ctx.TargetSpeed = Ctx.FightSpeed;
        }
        else
        {
            Ctx.TargetSpeed = Ctx.MoveSpeed;
        }
        SetPlayerMovement();

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
        else if (Ctx.IsLightAttackPressed && !Ctx.IsAttacking)
        {
            SwitchState(Factory.LightAttack());
        }
        else if(Ctx.IsHeavyAttackPressed&& !Ctx.IsAttacking)
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
