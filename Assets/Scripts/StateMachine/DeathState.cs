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
        Ctx.IsKnockedBack = true;
        Ctx.IsDead = true;        
        Ctx.Animator.SetBool(Ctx.AnimIDDeath, true);
        Ctx.Animator.Play($"Death", 0, 0);
        ExitAllAnimations();
        Ctx.StartCoroutine(TestRespawn());
    }

    public override void UpdateState()
    {
        //Debug.Log("DEATH state is currently active");
        Ctx.DebugCurrentSuperState = "Death State";
        CheckSwitchStates();

        if (Ctx.IsKnockedBack)
        {
            Ctx.SetHitKnockback();
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

    public IEnumerator TestRespawn()
    {
        yield return new WaitForSeconds(5f);
        SwitchState(Factory.FreeRoam());
        Ctx.Controller.enabled = true;
        Ctx.OnDeath?.Invoke(true);
    }
    public void ExitAllAnimations()
    {

    }
}
