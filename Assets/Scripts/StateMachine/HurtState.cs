using UnityEngine;

public class HurtState : BaseState
{
    public HurtState(StateMachine currentContext, StateFactory stateFactory)
     : base(currentContext, stateFactory){ }

    public override void EnterState()
    {
        Debug.LogWarning("Player has entered HURT state");
        Ctx.DebugCurrentSubState = "Hurt State";


        Ctx.IsHitLanded = false;
        Ctx.IsKnockedBack = true;
        Ctx.IsHurt = true;
        Ctx.Animator.SetBool(Ctx.AnimIDHurt, true);
        Ctx.Animator.Play($"LightHurt{Ctx.HitType}", 0, 0);
        ExitAllAnimations();
    }

    public override void UpdateState()
    {
        //Debug.Log("HURT state is currently active");

        CheckSwitchStates();
        
        if(Ctx.IsKnockedBack)
        {
            Ctx.SetHitKnockback();
        }
    }

    public override void ExitState()
    {
        //Debug.LogWarning("Player has exited HURT state");
        //Ctx.Animator.Play($"Hurt{Ctx.HitType}", 0, 0);
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsHurt)
        {
            SwitchState(Factory.Idle());
        }
        if(Ctx.IsHitLanded)
        {
            SwitchState(Factory.Hurt());
        }
     /*   if (Ctx.IsDodgePressed)
        {
            SwitchState(Factory.Dodge());
            Ctx.Animator.SetBool(Ctx.AnimIDHurt, false);
            Ctx.IsHurt = false;
        }*/
    }

    public override void InitializeSubStates()
    {

    }

    public void ExitAllAnimations()
    {
        Ctx.Animator.SetBool(Ctx.AnimIDLightAttack1, false);
        Ctx.Animator.SetBool(Ctx.AnimIDLightAttack2, false);
        Ctx.Animator.SetBool(Ctx.AnimIDLightAttack3, false);
        Ctx.Animator.SetBool(Ctx.AnimIDLightAttack4, false);
        Ctx.Animator.SetBool(Ctx.AnimIDLightAttack5, false);
        Ctx.Animator.SetBool(Ctx.AnimIDLightAttack6, false);
        Ctx.Animator.SetBool(Ctx.AnimIDLightAttack7, false);
    }
}
