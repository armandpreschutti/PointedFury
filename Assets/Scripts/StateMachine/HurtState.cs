using UnityEngine;

public class HurtState : BaseState
{
    public HurtState(StateMachine currentContext, StateFactory stateFactory)
     : base(currentContext, stateFactory){ }

    public override void EnterState()
    {
        //SsDebug.LogWarning("Player has entered HURT state");

        Ctx.SetIncomingAttackDirection();
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
        if(Ctx.IsHitLanded)
        {
            SwitchState(Factory.Hurt());
        }
        if(Ctx.IsBlockPressed && !Ctx.IsKnockedBack)
        {
            SwitchState(Factory.Block());
        }
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
    }
}
