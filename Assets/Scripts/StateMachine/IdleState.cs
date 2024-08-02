using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(StateMachine currentContext, StateFactory StateFactory)
    : base(currentContext, StateFactory) { }

    public override void EnterState()
    {
       // Debug.LogWarning("Player has entered IDLE state");


        Ctx.IsAttacking = false;
        //Ctx.OnIdle?.Invoke(true);
        if (!Ctx.IsAI && Ctx. EnemiesNearby != null && Ctx.EnemiesNearby.Length == 0)
        {
            Ctx.IsFighting = false;
            Ctx.OnFight?.Invoke(false);
            Ctx.LightAttackID = 0;
            Ctx.HeavyAttackID = 0;
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
        if (!Ctx.IsAI && Ctx.EnemiesNearby != null && Ctx.EnemiesNearby.Length == 0)
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
            else if (Ctx.IsHeavyAttackPressed && !Ctx.IsDepeleted)
            {
                SwitchState(Factory.HeavyAttack());
            }
            else if (Ctx.IsLightHitLanded)
            {
                if(Ctx.HitID == 0)
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
                if(Ctx.CurrentTarget != null && Ctx.CurrentTarget.GetComponent<StateMachine>().IsEvadable)
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
            else if(Ctx.IsDashPressed)
            {
                SwitchState(Factory.Dash());
            }
            else if(Ctx.IsFinishing)
            {
                SwitchState(Factory.Finishing());
            }
            else if(Ctx.IsFinished)
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
        if (Ctx.IsFighting || (Ctx.EnemiesNearby != null && Ctx.EnemiesNearby.Length > 0))
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
