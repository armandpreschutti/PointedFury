using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;

public class AIIDleState : AIBaseState
{
    public AIIDleState(AIBrain currentContext, AIStateFactory stateFactory)
    : base(currentContext, stateFactory) { }


    float stateTime;
    float attackTime/*e = Random.Range(0f, 1f);*/;
    float disengageTime;
    float approachTime;
    
    public override void EnterState()
    {
        //Debug.LogWarning("Player has entered IDLE state");
        
        Ctx.comboCount = 0;
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
            if (approachTime >= 1f)
            {
                SwitchState(Factory.Approaching());
            }
        }
        else if (Ctx.DistanceToPlayer(Ctx._currentTarget.transform) < (Ctx.targetDistance - Ctx.DistanceBuffer))
        {
            disengageTime += Time.deltaTime;
            if (disengageTime >= 1f)
            {
                SwitchState(Factory.Disengaging());
            }
        }
        else if (Ctx.isAttacker && /*stateTime > attackTime*/!Ctx.StateMachine.CurrentTarget.GetComponent<StateMachine>().IsAttacking && !Ctx.isHurt)
        {
            attackTime += Time.deltaTime;
            if (attackTime >= .75f /* Random.Range(.35f, .75f)*/)
            {
                SwitchState(Factory.Attack());
            }
         //   SwitchState(Factory.Attack());
        }
        else if (Ctx.isWatcher && stateTime > 3f && !Ctx.isHurt)
        {
            SwitchState(Factory.Strafing());
        }
        else if (Ctx.hitCount >= Ctx.HitTolerance)
        {
            SwitchState(Factory.Block());
        }
        /*  else if (Ctx.isHurt)
          {
              SwitchState(Factory.Hurt());
          }*/
    }

    public override void InitializeSubStates()
    {
     
    }
}
