using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeflectState : BaseState
{
    public DeflectState(StateMachine currentContext, StateFactory stateFactory)
      : base(currentContext, stateFactory) { }

    public override void EnterState()
    {
        //Debug.LogWarning("Player has entered DEFLECT state");

        // Ctx.SetIncomingAttackDirection();
        Ctx.SetBlockDirection();
        Ctx.IsDeflecting= true;
        Ctx.Animator.Play($"Deflect", 0, 0);
        Ctx.Animator.SetBool(Ctx.AnimIDDeflect, true);
    }

    public override void UpdateState()
    {
        //Debug.Log("DEFLECT state is currently active");
        Ctx.DebugCurrentSubState = "Deflect State";
        CheckSwitchStates();


    }

    public override void ExitState()
    {
        //Debug.LogWarning("Player has exited DEFLECT state");
        Ctx.IsDeflecting = false;
        Ctx.Animator.SetBool(Ctx.AnimIDDeflect, false);
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsDead)
        {
            if(!Ctx.IsDeflecting )
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
                if (Ctx.HitID == 0)
                {
                    SwitchState(Factory.Stunned());
                }
                else
                {
                    SwitchState(Factory.Hurt());
                }
            }
            else if (Ctx.IsHeavyHitLanded)
            {
                if (Ctx.HitID == 0)
                {
                    SwitchState(Factory.Stunned());
                }
                else
                {
                    SwitchState(Factory.Hurt());
                }
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
