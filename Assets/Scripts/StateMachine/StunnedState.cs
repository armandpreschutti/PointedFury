using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunnedState : BaseState
{
    public StunnedState(StateMachine currentContext, StateFactory stateFactory)
      : base(currentContext, stateFactory) { }

    public override void EnterState()
    {
        //Debug.LogWarning("Player has entered STUNNED state");

        
        Ctx.Animator.SetBool(Ctx.AnimIDStunned, true);
        Ctx.Animator.Play($"Stunned", 0, 0);
        Ctx.IsParryable = false;
        Ctx.IsParried = false;
        Ctx.IsEvadable = false;
       // Ctx.IsBlockPressed = false;
        Ctx.IsLightHitLanded = false;
        Ctx.IsHeavyHitLanded = false;
        Ctx.IsLightAttackPressed= false;
        Ctx.IsHeavyAttackPressed = false;
        Ctx.IsKnockedBack = true;
        Ctx.IsFighting = true;
        Ctx.OnFight?.Invoke(true);
        Ctx.IsStunned = true;
        ExitAllAnimations();
        Ctx.SetIncomingAttackDirection();
    }

    public override void UpdateState()
    {
        //Debug.Log("STUNNEDstate is currently active");
        Ctx.DebugCurrentSubState = "Stunned State";
        CheckSwitchStates();
        
        if (Ctx.IsKnockedBack)
        {
            Ctx.SetHitKnockBack();
        }

    }

    public override void ExitState()
    {
        //Debug.LogWarning("Player has exited STUNNED state");

        Ctx.Animator.SetBool(Ctx.AnimIDStunned, false);
        Ctx.IsStunned = false;
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsDead)
        {
            if (!Ctx.IsStunned)
            {
                if (Ctx.IsBlockPressed)
                {
                    SwitchState(Factory.Block());
                }
                else if (Ctx.MoveInput != Vector2.zero)
                {
                    SwitchState(Factory.Move());
                }

                else
                {
                    SwitchState(Factory.Idle());
                }
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

    public void ExitAllAnimations()
    {
        Ctx.Animator.SetBool(Ctx.AnimIDLightAttack, false);
        Ctx.Animator.SetBool(Ctx.AnimIDHeavyAttack, false);
    }
}
