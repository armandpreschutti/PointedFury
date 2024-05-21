
using UnityEngine;

public class AIAttackState : AIBaseState
{
    public AIAttackState(AIBrain currentContext, AIStateFactory stateFactory)
    : base(currentContext, stateFactory) { }


    int blockBreakChance;
    int heavyAttackChance;
    bool isChaining;
    float stateTime;
    public override void EnterState()
    {
         Debug.LogWarning("Enemmy has entered ATTACK state");
        stateTime = 0f;
        blockBreakChance = Random.Range(1, 11);
        heavyAttackChance = Random.Range(1, 11);
        if(!Ctx.StateMachine.IsLightAttackPressed && !Ctx.StateMachine.IsHeavyAttackPressed)
        {
            if (Ctx.HeavyAttackSkill >= heavyAttackChance)
            {
                Ctx.StateMachine.IsHeavyAttackPressed = true;
            }
            else
            {
                Ctx.StateMachine.IsLightAttackPressed = true;
            }
        }    

        Ctx.comboCount++;
      
    }

    public override void UpdateState()
    {
        // Debug.Log("ATTACK state is currently active");

        Ctx.DebugSubState = "Attack State";
        CheckSwitchStates();

        stateTime += Time.deltaTime;
        if (Ctx.comboCount < Ctx.ComboSkill && stateTime > .25f && Ctx.CanComboChain)
        {
            isChaining = true;
        }
    }

    public override void ExitState()
    {
        //Debug.LogWarning("Enemy has exited ATTACK state");
        
    }

    public override void CheckSwitchStates()
    {
        if (isChaining || (Ctx.comboCount < Ctx.ComboSkill && !Ctx.StateMachine.IsAttacking))
        {
            SwitchState(Factory.Attack());
        }
        else if (Ctx.isHurt)
        {
            SwitchState(Factory.Hurt());
        }
        else if(!Ctx.StateMachine.IsAttacking && Ctx.comboCount >= Ctx.ComboSkill) 
        {
            SwitchState(Factory.Idle());
        }
    }

    public override void InitializeSubStates()
    {

    }
}
