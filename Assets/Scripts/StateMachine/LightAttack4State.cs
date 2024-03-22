using UnityEngine;

public class LightAttack4State : BaseState
{
    public LightAttack4State(StateMachine currentContext, StateFactory stateFactory)
    : base(currentContext, stateFactory) { }


    public override void EnterState()
    {
        //Debug.LogWarning("Player has entered LIGHT ATTACK 4 state");
        Ctx.DebugCurrentSubState = "Light Attack 4 State";

        Ctx.AttackType = 4;
        Ctx.IsLightAttacking4 = true;
        Ctx.IsAttacking = true;
        Ctx.IsLightAttackPressed = false;
        Ctx.IsComboAttacking = false;
        Ctx.CanComboAttack = true;
        Ctx.IsFighting = true;
    }

    public override void UpdateState()
    {
        //Debug.Log("LIGHT ATTACK 4state is currently active");
        CheckSwitchStates();

        Ctx.SetAttackDirection();
        if (Ctx.IsCharging)
        {
            Ctx.ChargeAtEnemy();
        }
        if (Ctx.IsLightAttackPressed)
        {
            Ctx.IsComboAttacking = true;
            Ctx.CanComboAttack = false;
            Ctx.Animator.SetBool(Ctx.AnimIDLightAttack5, true);
        }
    }

    public override void ExitState()
    {
        //Debug.Log("Player has exited LIGHT ATTACK 4 state");

        Ctx.Animator.SetBool(Ctx.AnimIDLightAttack4, false);
        Ctx.IsAttacking = false;
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsLightAttacking4)
        {
            if (Ctx.IsComboAttacking)
            {
                SwitchState(Factory.LightAttack5());
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
    }

    public override void InitializeSubStates()
    {

    }
}
