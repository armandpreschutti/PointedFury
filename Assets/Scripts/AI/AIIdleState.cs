
using UnityEngine;

public class AIIDleState : AIBaseState
{
    public AIIDleState(AIBrain currentContext, AIStateFactory stateFactory)
    : base(currentContext, stateFactory) { }


    float stateTime;
    float initialAttackTime;
    float disengageTime;
    float approachTime;
    public override void EnterState()
    {
        //Debug.LogWarning("Player has entered IDLE state");
        
        Ctx.comboCount = 0;

        Ctx.hitCount = 0;
    }

    public override void UpdateState()
    {
        // Debug.Log("COMBAT state is currently active");
        Ctx.DebugSubState = "Idle State";
        CheckSwitchStates();

        stateTime += Time.deltaTime;
        Ctx.moveInput = Vector2.zero;
    }

    public override void ExitState()
    {
        //Debug.LogWarning("Player has exited COMBAT state");


    }

    public override void CheckSwitchStates()
    {

        if (Ctx.DistanceToPlayer(Ctx._currentTarget.transform) > (Ctx.targetDistance + Ctx.DistanceBuffer))
        {
            approachTime += Time.deltaTime;
            if (approachTime >= .5f)
            {
                SwitchState(Factory.Approaching());
            }
        }
        else if (Ctx.DistanceToPlayer(Ctx._currentTarget.transform) < (Ctx.targetDistance - Ctx.DistanceBuffer) && !Ctx._currentTarget.GetComponent<StateMachine>().IsAttacking)
        {
            disengageTime += Time.deltaTime;
            if (disengageTime >= .5f)
            {
                SwitchState(Factory.Disengaging());
            }
        }
        else if (Ctx.isAttacker && !Ctx.StateMachine.CurrentTarget.GetComponent<StateMachine>().IsAttacking && !Ctx.isHurt && Ctx.ComboSkill > 0)
        {
            Ctx.timeSinceAttack += Time.deltaTime;
            if (Ctx.timeSinceAttack >= Ctx.AttackInterval && !Ctx._initialAttack)
            {
                SwitchState(Factory.Attack());
            }
            else if (Ctx._initialAttack)
            {
                initialAttackTime += Time.deltaTime;
                if (initialAttackTime >= Ctx.InitialAttackDelay)
                {
                    SwitchState(Factory.Attack());
                }

            }
        }
        else if (Ctx.isWatcher && stateTime > Ctx._strafeInterval && !Ctx.isHurt)
        {
            SwitchState(Factory.Strafing());
        }
        else if (Ctx.hitCount >= Ctx.HitTolerance && Ctx.StateMachine.CurrentTarget.GetComponent<StateMachine>().IsAttacking && Ctx.StateMachine.CurrentTarget.GetComponent<StateMachine>().CurrentTarget == Ctx.gameObject)
        {
            SwitchState(Factory.Block());
        }
    }

    public override void InitializeSubStates()
    {
     
    }
}
