using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostAttackState : BaseState
{
    public PostAttackState(StateMachine currentContext, StateFactory stateFactory)
    : base(currentContext, stateFactory) { }


    public override void EnterState()
    {
        //Debug.LogWarning("Player has entered POST ATTACK state");

        Ctx.Animator.SetBool(Ctx.AnimIDPostAttack, true);
        Ctx.Animator.SetBool(Ctx.AnimIDHeavyAttack, false);
        Ctx.Animator.SetBool(Ctx.AnimIDLightAttack, false);
        Ctx.IsPostAttack = true;
        Ctx.IsFighting = true;
        Ctx.OnFight?.Invoke(true);
        //Ctx.SetAttackDirection();
    }

    public override void UpdateState()
    {
        //Debug.Log("POST ATTACK state is currently active");
        Ctx.DebugCurrentSubState = $"Post Attack State";
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        //Debug.LogWarning("Player has exited POST ATTACK state");

        Ctx.IsPostAttack = false;
        Ctx.Animator.SetBool(Ctx.AnimIDPostAttack, false);
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsDead)
        {
            if (!Ctx.IsPostAttack)
            {
                SwitchState(Factory.Idle());
            }
            else if (Ctx.IsLightAttackPressed)
            {
                SwitchState(Factory.LightAttack());
            }
            else if (Ctx.IsHeavyAttackPressed)
            {
                SwitchState(Factory.HeavyAttack());
            }
            else if (Ctx.IsBlockPressed)
            {
                SwitchState(Factory.Block());
            }
            else if (Ctx.IsLightHitLanded)
            {
                SwitchState(Factory.Hurt());
            }
            else if (Ctx.IsHeavyHitLanded)
            {
                SwitchState(Factory.Hurt());
            }
            else if (Ctx.IsParrySucces)
            {
                SwitchState(Factory.Parry());
            }
            else if (Ctx.IsEvadeSucces)
            {
                SwitchState(Factory.Evade());
            }
            else if (Ctx.IsParried)
            {
                SwitchState(Factory.Stunned());
            }
            else if (Ctx.IsDashPressed)
            {
                SwitchState(Factory.Dash());
            }
            else if (Ctx.MoveInput != Vector2.zero)
            {
                SwitchState(Factory.Move());
            }
        }
      
    }

    public override void InitializeSubStates()
    {

    }

  
}
