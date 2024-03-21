using UnityEngine;

public class HurtState : BaseState
{
    public HurtState(StateMachine currentContext, StateFactory stateFactory)
     : base(currentContext, stateFactory){ }

    public override void EnterState()
    {
        Debug.LogWarning("Player has entered HURT state");
        Ctx.DebugCurrentSubState = "Hurt State";

        Ctx.HitLanded = false;
        Ctx.IsHurt = true;
        Ctx.Animator.SetBool(Ctx.AnimIDHurt, true);
        Ctx.Animator.Play($"LightHurt{Ctx.HitType}", 0, 0);
    }

    public override void UpdateState()
    {
        //Debug.Log("HURT state is currently active");

        CheckSwitchStates();
        if (Ctx.VerticalVelocity < 0.0f)
        {
            Ctx.VerticalVelocity = -2f;
        }        
    }

    public override void ExitState()
    {
        Debug.LogWarning("Player has exited HURT state");
        //Ctx.Animator.Play($"Hurt{Ctx.HitType}", 0, 0);
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsHurt)
        {
            SwitchState(Factory.Idle());
        }
        if(Ctx.HitLanded)
        {
            SwitchState(Factory.Hurt());
        }
    }

    public override void InitializeSubStates()
    {

    }
}
