using UnityEngine;

public class LightAttack3State : BaseState
{
    public LightAttack3State(StateMachine currentContext, StateFactory stateFactory)
    : base(currentContext, stateFactory) { }


    public override void EnterState()
    {
        Debug.LogWarning("Player has entered LIGHT ATTACK 2 state");
        Ctx.DebugCurrentSubState = "Light Attack 2 State";

        //Ctx.IsFighting = true;
        Ctx.AttackType = 3;
        Ctx.IsLightAttacking3 = true;
        Ctx.IsAttacking = true;
        Ctx.IsLightAttackPressed = false;
        Ctx.IsComboAttacking = false;
        Ctx.CanComboAttack = true;
        Ctx.IsFighting = true;
        Ctx.RotateTowardTarget();
    }

    public override void UpdateState()
    {
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
            Ctx.Animator.SetBool(Ctx.AnimIDLightAttack4, true);
        }
    }

    public override void ExitState()
    {
        Debug.Log("Player has exited LIGHT ATTACK 2 state");

        //Ctx.OnLightAttack2?.Invoke(true);
        Ctx.Animator.SetBool(Ctx.AnimIDLightAttack3, false);
        Ctx.IsAttacking = false;
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsLightAttacking3)
        {
            if (Ctx.IsComboAttacking)
            {
                SwitchState(Factory.LightAttack4());
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
