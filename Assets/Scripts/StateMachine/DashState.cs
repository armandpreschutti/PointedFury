using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : BaseState
{
    public DashState(StateMachine currentContext, StateFactory stateFactory)
     : base(currentContext, stateFactory) { }


    public override void EnterState()
    {
        //Debug.LogWarning("Player has entered DASH state");


        Ctx.Animator.SetBool(Ctx.AnimIDDash, true);
        Ctx.IsDashing = true;
        Ctx.IsFighting= true;
        Ctx.OnFight?.Invoke(true);
        Ctx.OnDash?.Invoke(true);
        Ctx.IsDashPressed = false;
        Ctx.SetDashDirection();
    }

    public override void UpdateState()
    {
        //Debug.Log("DASH state is currently active");
        Ctx.DebugCurrentSubState = "Dash State";

        CheckSwitchStates();
        Ctx.MoveInput = Vector2.zero;
       // Ctx.DashMovement();
    }

    public override void ExitState()
    {
        //Debug.LogWarning("Player has exited state");

        Ctx.Animator.SetBool(Ctx.AnimIDDash, false);
        Ctx.OnDash?.Invoke(false);
        Ctx.IsDashing = false;
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsDead)
        {
            if (!Ctx.IsDashing)
            {
                if (Ctx.IsBlockPressed)
                {
                    SwitchState(Factory.Block());
                }
                else if (Ctx.IsLightAttackPressed)
                {
                    Ctx.IsHeavyAttackPressed = false;
                   // Ctx.IsSprintAttack = true;
                    SwitchState(Factory.LightAttack());
                }
                else if (Ctx.IsHeavyAttackPressed && !Ctx.IsDepeleted)
                {
                    Ctx.IsLightAttackPressed = false;
                   // Ctx.IsSprintAttack = true;
                    SwitchState(Factory.HeavyAttack());
                }
                else
                {
                    if (Ctx.MoveInput != Vector2.zero)
                    {
                        if (Ctx.IsSprintPressed && !Ctx.IsDepeleted)
                        {
                            SwitchState(Factory.Sprint());
                          //  Debug.LogWarning("DashTransitionCalled");
                        }
                        else
                        {
                            SwitchState(Factory.Move());
                        }

                    }
                    else
                    {
                        SwitchState(Factory.Idle());
                    }
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
            else if (Ctx.IsParried)
            {
                SwitchState(Factory.Stunned());
            }
            
            else if (Ctx.IsFinishing)
            {
                SwitchState(Factory.Finishing());
            }
            else if (Ctx.IsFinished)
            {
                SwitchState(Factory.Finished());
            }

        }
        else if (Ctx.IsParrySucces)
        {
            SwitchState(Factory.Parry());
        }
        
    }

    public override void InitializeSubStates()
    {

    }
}
