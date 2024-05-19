
using UnityEngine;

public class AIBlockState : AIBaseState
{
    public AIBlockState(AIBrain currentContext, AIStateFactory stateFactory)
    : base(currentContext, stateFactory) { }


    float stateTime;
    float postBlockTime;
    int parryChance;
    public override void EnterState()
    {
        //Debug.LogWarning("Enemy has entered BLOCK state");
        Ctx.StateMachine.IsBlockPressed= true;
        Ctx.comboCount = 0;
        parryChance = Random.Range(1, 11);
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

        Ctx.StateMachine.IsBlockPressed = false;

    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.StateMachine.CurrentTarget.GetComponent<StateMachine>().IsAttacking)
        {
            postBlockTime += Time.deltaTime;
            if(postBlockTime > 1f)
            {
                Ctx.StateMachine.IsBlockPressed = false;
                Ctx.hitCount = 0;
                SwitchState(Factory.Idle());
            }
        }
        else if (Ctx.StateMachine.CurrentTarget.GetComponent<StateMachine>().IsParryable && Ctx.StateMachine.CurrentTarget.GetComponent<StateMachine>().AttackType == "Heavy" && Ctx.ParrySkill >= parryChance &&!Ctx.StateMachine.IsStunned)
        {
            Ctx.StateMachine.OnAttemptParry?.Invoke();

        }
    }

    public override void InitializeSubStates()
    {

    }
}
