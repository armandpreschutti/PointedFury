using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeState : BaseState
{
    public EvadeState(StateMachine currentContext, StateFactory stateFactory)
     : base(currentContext, stateFactory) { }


    public override void EnterState()
    {
        //Debug.LogWarning("Player has entered DASH state");
        Ctx.Animator.SetBool(Ctx.AnimIDEvade, true);
        Ctx.IsEvading = true;
        Ctx.IsFighting = true;
        Ctx.IsEvadePressed = false;
        Ctx.IsEvadeSucces = false;
        Ctx.OnEvade?.Invoke(true);
        Ctx.SetIncomingAttackDirection();
    }

    public override void UpdateState()
    {
        //Debug.Log("DASH state is currently active");
        Ctx.DebugCurrentSubState = "Evade State";
        CheckSwitchStates();
        //Ctx.SetIncomingAttackDirection();
/*        if (Ctx.IsEvading)
        {
            Ctx.SetIncomingAttackDirection();
        }*/
    }

    public override void ExitState()
    {
        //Debug.LogWarning("Player has exited state");

        Ctx.Animator.SetBool(Ctx.AnimIDEvade, false);
        Ctx.IsEvading = false;
        Ctx.OnEvade?.Invoke(false);
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsDead)
        {
            if (!Ctx.IsEvading)
            {
                if (Ctx.IsBlockPressed)
                {
                    SwitchState(Factory.Block());
                }
                else if (Ctx.IsLightAttackPressed)
                {
                    Ctx.IsHeavyAttackPressed = false;
                    SwitchState(Factory.LightAttack());
                }
                else if (Ctx.IsHeavyAttackPressed && !Ctx.IsDepeleted)
                {
                    Ctx.IsLightAttackPressed = false;
                    SwitchState(Factory.HeavyAttack());
                }
                else if (Ctx.IsDashPressed)
                {
                    SwitchState(Factory.Dash());
                }
                else if (Ctx.IsFinishing)
                {
                    SwitchState(Factory.Finishing());
                }
                else if (Ctx.IsFinished)
                {
                    SwitchState(Factory.Finished());
                }
                else
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
            }         
            else if(Ctx.IsParrySucces)
            {
                SwitchState(Factory.Parry());
            }
        }
       
    }

    public override void InitializeSubStates()
    {

    }
}
