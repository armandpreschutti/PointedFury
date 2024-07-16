using UnityEngine;

public class MoveState : BaseState
{
    public MoveState(StateMachine currentContext, StateFactory stateFactory)
    : base(currentContext, stateFactory) { }

    public override void EnterState()
    {
        //Debug.LogWarning("Player has entered MOVE state");


        Ctx.IsAttacking = false;
        if (!Ctx.IsAI && Ctx.EnemiesNearby.Count == 0)
        {
            Ctx.IsFighting = false;
            Ctx.OnFight?.Invoke(false);
            Ctx.LightAttackID = 0;
            Ctx.HeavyAttackID = 0;
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
        if(!Ctx.IsDashing && !Ctx.IsEvading)
        {
            SetPlayerMovement();
        }


    }

    public override void ExitState()
    {
        //Debug.LogWarning("Player has exited MOVE state");

    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsDead)
        {
            if (Ctx.MoveInput == Vector2.zero)
            {
                SwitchState(Factory.Idle());
            }
            else if (Ctx.IsLightAttackPressed && !Ctx.IsAttacking)
            {
                SwitchState(Factory.LightAttack());
            }
            else if (Ctx.IsHeavyAttackPressed && !Ctx.IsAttacking)
            {
                SwitchState(Factory.HeavyAttack());
            }
            else if (Ctx.IsLightHitLanded)
            {
                if (Ctx.HitID == 0)
                {
                    SwitchState(Factory.Stunned());
                }
                else
                {
                    SwitchState(Factory.Hurt());
                }
            }
            else if (Ctx.IsHeavyHitLanded)
            {
                if (Ctx.HitID == 0)
                {
                    SwitchState(Factory.Stunned());
                }
                else
                {
                    SwitchState(Factory.Hurt());
                }
            }
            else if (Ctx.IsEvadeSucces)
            {
                SwitchState(Factory.Evade());
            }
            else if (Ctx.IsBlockPressed)
            {
                if (Ctx.CurrentTarget != null && Ctx.CurrentTarget.GetComponent<StateMachine>().IsEvadable && Ctx.CurrentTarget.GetComponent<StateMachine>().IsAI)
                {
                    SwitchState(Factory.Deflect());
                }
                else
                {
                    SwitchState(Factory.Block());
                }
            }
            else if (Ctx.IsParrySucces)
            {
                SwitchState(Factory.Parry());
            }
            else if (Ctx.IsParried)
            {
                SwitchState(Factory.Stunned());
            }
            else if (Ctx.IsDashPressed)
            {
                SwitchState(Factory.Dash());
            }
            else if (Ctx.IsFinishing)
            {
                SwitchState(Factory.Finishing());
            }
            else if (Ctx.IsFinished)
            {
                SwitchState(Factory.Finished());
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
