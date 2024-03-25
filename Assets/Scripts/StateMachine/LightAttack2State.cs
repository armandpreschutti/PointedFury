using UnityEngine;

public class LightAttack2State : BaseState
{
    public LightAttack2State(StateMachine currentContext, StateFactory stateFactory)
    : base(currentContext, stateFactory) { }


    public override void EnterState()
    {
        //Debug.LogWarning("Player has entered LIGHT ATTACK 2 state");
        Ctx.DebugCurrentSubState = "Light Attack 2 State";

        Ctx.AttackType = 2;
        Ctx.IsLightAttacking2 = true;
        Ctx.IsAttacking = true;
        Ctx.IsLightAttackPressed = false;
        Ctx.IsComboAttacking = false;
        Ctx.CanComboAttack = true;
        Ctx.IsFighting = true;

    }

    public override void UpdateState()
    {
        //Debug.Log("LIGHT ATTACK 2 state is currently active");
        CheckSwitchStates();

        Ctx.SetAttackDirection();
        if (Ctx.IsCharging)
        {
            Ctx.LightAttackMovement();
        }
        if (Ctx.IsLightAttackPressed && !Ctx.IsHurt)
        {
            Ctx.IsComboAttacking = true;
            Ctx.CanComboAttack = false;
            Ctx.Animator.SetBool(Ctx.AnimIDLightAttack3, true);
        }
    }

    public override void ExitState()
    {
        //Debug.Log("Player has exited LIGHT ATTACK 2 state");

        Ctx.Animator.SetBool(Ctx.AnimIDLightAttack2, false);
        Ctx.IsAttacking = false;
    }

    public override void CheckSwitchStates()
    {
        Ctx.FightTimeoutActive = true;
        Ctx.FightTimeoutDelta = Ctx.AttackTimeout;
        if (!Ctx.IsLightAttacking2)
        {
            if (Ctx.IsComboAttacking)
            {
                SwitchState(Factory.LightAttack3());
            }
            else
            {
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
        if (Ctx.IsHitLanded)
        {
            SwitchState(Factory.Hurt());
            Ctx.IsLightAttacking2 = false;
/*            Ctx.Animator.SetBool(Ctx.AnimIDLightAttack2, false);
            Ctx.Animator.SetBool(Ctx.AnimIDLightAttack3, false);*/
        }
    }

    public override void InitializeSubStates()
    {

    }
}
