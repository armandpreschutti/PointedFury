using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintState : BaseState
{
    public SprintState(StateMachine currentContext, StateFactory stateFactory)
     : base(currentContext, stateFactory) { }


    public override void EnterState()
    {
        Debug.LogWarning("Player has entered SPRINT state");

        Ctx.IsSprinting= true;
        Ctx.OnSprint?.Invoke(true);
        Ctx.Animator.SetBool(Ctx.AnimIDSprint, true);
        Ctx.LightAttackID = 0;
        Ctx.HeavyAttackID = 0;
    }

    public override void UpdateState()
    {
        //Debug.Log("SPRINT state is currently active");
        Ctx.DebugCurrentSubState = "Sprint State";
        CheckSwitchStates();

        Ctx.SprintMovement();
    }

    public override void ExitState()
    {
        Debug.LogWarning("Player has exited SPRINT state");
        Ctx.IsSprinting = false;
        Ctx.Animator.SetBool(Ctx.AnimIDSprint, false);
        Ctx.OnSprint?.Invoke(false);
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsDead)
        {
            if (!Ctx.IsSprintPressed)
            {
                SwitchState(Factory.Move());
            }
            else if(Ctx.MoveInput == Vector2.zero)
            {
                SwitchState(Factory.Idle());
            }
            else if (Ctx.IsLightAttackPressed)
            {
                Ctx.IsHeavyAttackPressed = false;
                Ctx.IsSprintAttack = true;
                SwitchState(Factory.LightAttack());
            }
            else if (Ctx.IsHeavyAttackPressed)
            {
                Ctx.IsLightAttackPressed = false;
                Ctx.IsSprintAttack = true;
                SwitchState(Factory.HeavyAttack());
            }
            else if (Ctx.IsBlockPressed)
            {
                if (Ctx.EnemiesNearby.Count > 0)
                {
                    SwitchState(Factory.Deflect());
                }
                else
                {
                    SwitchState(Factory.Block());
                }
            }

            /*else if (Ctx.IsLightHitLanded)
            {
                SwitchState(Factory.Hurt());
            }
            else if (Ctx.IsHeavyHitLanded)
            {
                SwitchState(Factory.Hurt());
            }
            else if (Ctx.IsParried)
            {
                SwitchState(Factory.Stunned());
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
            else if (Ctx.IsFinishing)
            {
                SwitchState(Factory.Finishing());
            }
            else if (Ctx.IsFinished)
            {
                SwitchState(Factory.Finished());
            }*/


        }

    }

    public override void InitializeSubStates()
    {

    }
}
