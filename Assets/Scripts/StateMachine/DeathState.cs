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
       /* Ctx.SetIncomingAttackDirection();
        Ctx.Animator.SetBool(Ctx.AnimIDDeath, true);
        Ctx.Animator.Play($"Death", 0, 0);*/
        /*  if(!Ctx.IsFinished)
          {
              Ctx.SetIncomingAttackDirection();
              Ctx.Animator.SetBool(Ctx.AnimIDDeath, true);
              Ctx.Animator.Play($"Death", 0, 0);
          }*/
         Ctx.OnEnableRagdoll?.Invoke(Ctx.CurrentTarget.transform.position, /*250f,*/ Ctx.HitType, Ctx.HitID);

    }

    public override void UpdateState()
    {
        //Debug.Log("DEATH state is currently active");
        Ctx.DebugCurrentSuperState = "Death State";
        CheckSwitchStates();

/*        if (Ctx.IsKnockedBack)
        {

            //   Time.timeScale = .25f;
            //Ctx.SetHitKnockBack();
            Ctx.Controller.detectCollisions = false;
        }
        else
        {
            if (Ctx.IsAI)
            {
                Ctx.GetComponent<AIBrain>().enabled = false;
            }
            Ctx.Controller.enabled = false;
            Ctx.enabled = false;
            Time.timeScale = 1f;
        }*/
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
