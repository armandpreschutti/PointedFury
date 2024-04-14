using System.Collections;
using UnityEngine;

public class DeathState : BaseState
{
    public DeathState(StateMachine currentContext, StateFactory stateFactory)
      : base(currentContext, stateFactory) 
    {
        IsRootState = true;
        InitializeSubStates();
    }

    public override void EnterState()
    {
        //Debug.LogWarning("Player has entered DEATH state");

        Ctx.SetIncomingAttackDirection();
        Ctx.IsLightHitLanded = false;
        Ctx.IsHeavyHitLanded = false;
        Ctx.IsPostAttack = false;
        Ctx.IsAttacking = false;
        Ctx.IsKnockedBack = true;
        Ctx.IsDead = true;        
        Ctx.Animator.SetBool(Ctx.AnimIDDeath, true);
        Ctx.Animator.Play($"Death", 0, 0);
        ExitAllAnimations();
    }

    public override void UpdateState()
    {
        //Debug.Log("DEATH state is currently active");
        Ctx.DebugCurrentSuperState = "Death State";
        CheckSwitchStates();

        if (Ctx.IsKnockedBack)
        {
            Ctx.SetHitKnockBack();
        }
        else
        {
            Ctx.Controller.enabled = false;
        }
    }

    public override void ExitState()
    {
        //Debug.LogWarning("Player has exited DEATH state");

        Ctx.Animator.SetBool(Ctx.AnimIDDeath, false);
        Ctx.IsDead = false;
        Ctx.IsKnockedBack = false;
    }

    public override void CheckSwitchStates()
    {
       
    }

    public override void InitializeSubStates()
    {
        SetSubState(Factory.Idle());
    }

    public void ExitAllAnimations()
    {

    }
}
