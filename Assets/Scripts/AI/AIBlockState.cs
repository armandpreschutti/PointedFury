using UnityEditor.Search;
using UnityEngine;

public class AIBlockState : AIBaseState
{
    public AIBlockState(AIBrain currentContext, AIStateFactory stateFactory)
    : base(currentContext, stateFactory) { }


    float stateTime;
    float attackTime = Random.Range(1f, 4f);

    public override void EnterState()
    {
        //Debug.LogWarning("Enemy has entered BLOCK state");
        Ctx.StateMachine.IsBlockPressed= true;
        Ctx.comboCount = 0;
/*        Ctx.hitCount = 0;*/
    }

    public override void UpdateState()
    {
        // Debug.Log("BLOCK state is currently active");
        Ctx.DebugSubState = "Block State";
        CheckSwitchStates();

        stateTime += Time.deltaTime;
        Ctx.moveInput = Vector2.zero;   
    }

    public override void ExitState()
    {
        //Debug.LogWarning("Enemy has exited BLOCK state");

        //Ctx.StateMachine.IsBlockPressed = false;
    }

    public override void CheckSwitchStates()
    {
        if (Ctx.StateMachine.IsBlockSuccess)
        {
            SwitchState(Factory.Block());
        }
        else if (stateTime > 1f && !Ctx.StateMachine.CurrentTarget.GetComponent<StateMachine>().IsAttacking)
        {
            Ctx.StateMachine.IsBlockPressed = false;
            Ctx.hitCount = 0;
            SwitchState(Factory.Idle());
        }
        else if (Ctx.StateMachine.CurrentTarget.GetComponent<StateMachine>().IsParryable && Ctx.StateMachine.CurrentTarget.GetComponent<StateMachine>().AttackType == "Heavy")
        {
            Ctx.StateMachine.OnAttemptParry?.Invoke();
        }

        /* else if (Ctx.isHurt)
         {
             SwitchState(Factory.Hurt());
         }*/
    }

    public override void InitializeSubStates()
    {

    }
}
