
using UnityEngine;

public class AIStrafingState : AIBaseState
{
    public AIStrafingState(AIBrain currentContext, AIStateFactory stateFactory)
    : base(currentContext, stateFactory) { }

    
    float stateTime;
    int strafeDirection;
    float initialAttackTime;
    float disengageTime;
    float approachTime;
    public override void EnterState()
    {
       // Debug.LogWarning("Enemmy has entered STRAFING state");

        SetRandomStrafeDirection();
    }

    public override void UpdateState()
    {

        // Debug.Log("STRAFING state is currently active");
        Ctx.DebugSubState = "Strafing State";
        CheckSwitchStates();

        Ctx.moveInput.x = strafeDirection;
        stateTime += Time.deltaTime;
    }

    public override void ExitState()
    {
        //Debug.LogWarning("Enemy has exited STRAFING state");


    }

    public override void CheckSwitchStates()
    {
        if(stateTime > Ctx._strafeInterval)
        {
            SwitchState(Factory.Idle());
        }
        if (Ctx.DistanceToPlayer(Ctx._currentTarget.transform) > (Ctx.targetDistance + Ctx.DistanceBuffer))
        {
            approachTime += Time.deltaTime;
            if (approachTime >= .25f)
            {
                SwitchState(Factory.Approaching());
            }
        }
        else if (Ctx.DistanceToPlayer(Ctx._currentTarget.transform) < (Ctx.targetDistance - Ctx.DistanceBuffer) && !Ctx._currentTarget.GetComponent<StateMachine>().IsAttacking)
        {
            disengageTime += Time.deltaTime;
            if (disengageTime >= .25f)
            {
                SwitchState(Factory.Disengaging());
            }
        }
        else if(Ctx.isHurt)
        {
            SwitchState(Factory.Hurt());
        }
    }

    public override void InitializeSubStates()
    {

    }
    public void SetRandomStrafeDirection()
    {
        strafeDirection = Random.Range(-1, 2);
        if (strafeDirection == 0)
        {
            SetRandomStrafeDirection();
        }
    }
}
