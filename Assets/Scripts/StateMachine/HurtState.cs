using UnityEngine;

public class HurtState : BaseState
{
    public HurtState(StateMachine currentContext, StateFactory stateFactory)
     : base(currentContext, stateFactory){ }

    public override void EnterState()
    {
        //Debug.LogWarning("Player has entered HURT state");

        Ctx.SetIncomingAttackDirection();
        Ctx.IsLightHitLanded = false;
        Ctx.IsHeavyHitLanded = false;
        Ctx.IsPostAttack = false;
        Ctx.IsAttacking = false;
        Ctx.IsKnockedBack = true;
        Ctx.IsBlocking= false;  
        Ctx.IsHurt = true;
        Ctx.IsFighting = true;
        Ctx.Animator.SetBool(Ctx.AnimIDHurt, true);
        Ctx.Animator.Play($"LightHurt{Ctx.HitID}", 0, 0);
        ExitAllAnimations();
    }

    public override void UpdateState()
    {
        //Debug.Log("HURT state is currently active");
        Ctx.DebugCurrentSubState = "Hurt State";
        CheckSwitchStates();
        
        if(Ctx.IsKnockedBack)
        {
            Ctx.SetHitKnockBack();
        }
    }

    public override void ExitState()
    {
        //Debug.LogWarning("Player has exited HURT state");

        Ctx.Animator.SetBool(Ctx.AnimIDHurt, false);
        Ctx.IsHurt = false;
        if (!Ctx.IsDead)
        {
            Ctx.IsKnockedBack = false;
        }

    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsHurt)
        {
            if (Ctx.MoveInput != Vector2.zero)
            {
                SwitchState(Factory.Move());
            }
            else
            {
                SwitchState(Factory.Idle());
            }
        }
        if(Ctx.IsLightHitLanded)
        {
            SwitchState(Factory.Hurt());
        }
        if(Ctx.IsHeavyHitLanded)
        {
            SwitchState(Factory.Stunned());
        }
       /* if(Ctx.IsBlockPressed && !Ctx.IsKnockedBack)
        {
            SwitchState(Factory.Block());
        }*/
        if(Ctx.IsParrySucces)
        {
            SwitchState(Factory.Parry());
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
