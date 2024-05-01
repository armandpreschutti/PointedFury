using UnityEngine;

public class HurtState : BaseState
{
    public HurtState(StateMachine currentContext, StateFactory stateFactory)
     : base(currentContext, stateFactory){ }

    public override void EnterState()
    {
        //Debug.LogWarning("Player has entered HURT state");

        Ctx.SetIncomingAttackDirection();
/*        Ctx.LightAttackID = 0;
        Ctx.HeavyAttackID = 0;*/
        Ctx.IsLightHitLanded = false;
        Ctx.IsHeavyHitLanded = false;
        Ctx.IsPostAttack = false;
        Ctx.IsAttacking = false;
        Ctx.IsKnockedBack = true;
        Ctx.IsBlocking= false;  
        Ctx.IsHurt = true;
        Ctx.IsFighting = true;
        Ctx.OnFight?.Invoke(true);
       // Ctx.Animator.SetBool(Ctx.AnimIDLightHurt, true);
        Ctx.Animator.Play($"{Ctx.HitType}Hurt{Ctx.HitID}", 0, 0);
        Ctx.Animator.SetInteger(Ctx.AnimIDHurtID, Ctx.HitID);
        Ctx.Animator.SetBool(Ctx.AnimIDHurt, true);
      /*  if (Ctx.HitType == "Light")
        {
            Ctx.Animator.SetBool(Ctx.AnimIDLightHurt, true);
        }
        else
        {
            Ctx.Animator.SetBool(Ctx.AnimIDHeavyHurt, true);
        }*/
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
        //Ctx.Animator.SetBool(Ctx.AnimIDHeavyHurt, false);
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
            if(Ctx.IsBlocking)
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
        else if(Ctx.IsLightHitLanded)
        {
            SwitchState(Factory.Hurt());
        }
        else if(Ctx.IsHeavyHitLanded)
        {
            SwitchState(Factory.Hurt());
        }
/*        else if (Ctx.IsParrySucces)
        {
            SwitchState(Factory.Parry());
        }*/
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
