using System.Collections;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

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


        Ctx.IsLightHitLanded = false;
        Ctx.IsHeavyHitLanded = false;
        Ctx.IsPostAttack = false;
        Ctx.IsAttacking = false;
        Ctx.IsKnockedBack = true;
        Ctx.IsStunned = false;
        Ctx.IsDead = true;
        Ctx.GameOver();
        Ctx.OnEnableRagdoll?.Invoke(Ctx.CurrentTarget == null ? Vector3.up : Ctx.CurrentTarget.transform.position, Ctx.HitType, Ctx.HitID);
    }

    public override void UpdateState()
    {
        //Debug.Log("DEATH state is currently active");
        Ctx.DebugCurrentSuperState = "Death State";
        CheckSwitchStates();

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

}
