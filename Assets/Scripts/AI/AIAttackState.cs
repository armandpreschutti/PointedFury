using UnityEditor.Search;
using UnityEngine;

public class AIAttackState : AIBaseState
{
    public AIAttackState(AIBrain currentContext, AIStateFactory stateFactory)
    : base(currentContext, stateFactory) { }


    //float stateTime;
    public override void EnterState()
    {
        
        // Debug.LogWarning("Enemmy has entered ATTACK state");
      /*  if(Ctx._currentTarget.GetComponent<StateMachine>().IsBlocking)
        {
            Ctx.StateMachine.IsHeavyAttackPressed= true;
        }
        else
        {
            Ctx.StateMachine.IsLightAttackPressed = true;    
        }*/
        Ctx.StateMachine.IsLightAttackPressed = true;
        Ctx.comboCount++;
    }

    public override void UpdateState()
    {
        // Debug.Log("ATTACK state is currently active");

        //stateTime += Time.deltaTime;
        Ctx.DebugSubState = "Attack State";
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        //Debug.LogWarning("Enemy has exited ATTACK state");

        //Ctx.StateMachine.IsLightAttackPressed = false;
    }

    public override void CheckSwitchStates()
    {
        if(Ctx.comboCount < Ctx.ComboSkill && !Ctx.StateMachine.IsAttacking)
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
