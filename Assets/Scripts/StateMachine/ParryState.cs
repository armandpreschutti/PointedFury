using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ParryState : BaseState
{
    public ParryState(StateMachine currentContext, StateFactory stateFactory)
    : base(currentContext, stateFactory) { }


    public override void EnterState()
    {
        //Debug.LogWarning("Player has entered PARRY state");

        SetParryType();

        Ctx.IsParrying = true;
        Ctx.IsAttacking = false;
        Ctx.Animator.SetBool(Ctx.AnimIDParry, true);
        Ctx.Animator.SetInteger(Ctx.AnimIDParryID, Ctx.ParryID);
        Ctx.IsFighting = true;
        Ctx.OnFight?.Invoke(true);
        Ctx.IsParrySucces = false;
        Ctx.IsEvadable = true;

    }

    public override void UpdateState()
    {
        //Debug.Log("PARRY state is currently active");
        Ctx.DebugCurrentSubState = $"Parry State";
        CheckSwitchStates();

        if(Ctx.IsCharging)
        {
            Ctx.SetIncomingAttackDirection();
            Ctx.ParryMovement();
        }

    }

    public override void ExitState()
    {
        //Debug.LogWarning("Player has exited PARRY state");
        Time.timeScale = 1f;
        Ctx.Animator.SetBool(Ctx.AnimIDParry, false);
        Ctx.IsParrying = false;
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsDead)
        {
            if (!Ctx.IsParrying)
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
                else if (Ctx.IsHeavyAttackPressed)
                {
                    Ctx.IsLightAttackPressed = false;
                    SwitchState(Factory.HeavyAttack());
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
    }

    public override void InitializeSubStates()
    {

    }

    public void SetParryType()
    {
        switch (Ctx.ParryID)
        {
            case 0:
                Ctx.ParryID = 1;
                break;
            case 1:
                Ctx.ParryID = 2;
                break;
            case 2:
                Ctx.ParryID = 3;
                break;
            case 3:
                Ctx.ParryID = 4;
                break;
            case 4:
                Ctx.ParryID = 1;
                break;
            default:
                break;
        }
    }


}
