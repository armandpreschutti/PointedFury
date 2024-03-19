using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class LightAttack1Sate : BaseState
{
    public LightAttack1Sate(StateMachine currentContext, StateFactory stateFactory)
    : base(currentContext, stateFactory) { }


    public override void EnterState()
    {
        Debug.LogWarning("Player has entered LIGHT ATTACK 1 state");
        Ctx.DebugCurrentSubState = "Light Attack 1 State";

        //Ctx.IsFighting = true;
        Ctx.AttackType = 1;
        Ctx.Animator.SetBool(Ctx.AnimIDLightAttack1, true);

        Ctx.IsLightAttacking1= true;
        Ctx.IsAttacking = true;
        Ctx.IsLightAttackPressed = false;
        Ctx.IsComboAttacking = false;
        Ctx.CanComboAttack = true;
        Ctx.IsFighting= true;
        Ctx.RotateTowardTarget();
    }

    public override void UpdateState()
    {
        //Debug.Log("LIGHT ATTACK state is currently active");
        
        CheckSwitchStates();

        Ctx.TargetSpeed = 0f;
        if (Ctx.IsCharging)
        {
            Ctx.ChargeAtEnemy();
        }
        if (Ctx.IsLightAttackPressed)
        {
            Ctx.IsComboAttacking = true;
            Ctx.CanComboAttack = false;
            Ctx.Animator.SetBool(Ctx.AnimIDLightAttack2, true);
            //Ctx.IsLightAttackPressed= false;
        }
    }

    public override void ExitState()
    {
        //Debug.Log("Player has exited LIGHT ATTACK state");

        Ctx.Animator.SetBool(Ctx.AnimIDLightAttack1, false);
        Ctx.IsAttacking = false;

    }

    public override void CheckSwitchStates()
    {
        //Ctx.CanComboAttack = false;
        //Ctx.ChargeAtEnemy();

        if (!Ctx.IsLightAttacking1)
        {
            if(Ctx.IsComboAttacking)
            {
                SwitchState(Factory.LightAttack2());

            }
            else
            {
                //Ctx.IsFighting = false;
                Ctx.FightTimeoutActive = true;
                Ctx.FightTimeoutDelta = Ctx.AttackTimeout;
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

        /* if (!Ctx.IsLightAttacking1)
         {
             if(Ctx.IsComboAttacking)
             {
                 SwitchState(Factory.LightAttack2());
             }
             else
             {
                 Ctx.CanComboAttack = false;
                 Ctx.FightTimeoutActive = true;
                 Ctx.FIghtTimeoutDelta = Ctx.AttackTimeout;
                 if (Ctx.MoveInput != Vector2.zero)
                 {
                     SwitchState(Factory.Move());
                 }
                 else
                 {
                     SwitchState(Factory.Idle());
                 }
             }   
         }        */
    }

    public override void InitializeSubStates()
    {

    }
}
