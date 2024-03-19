using UnityEngine;

public class LightAttack2State : BaseState
{
    public LightAttack2State(StateMachine currentContext, StateFactory stateFactory)
    : base(currentContext, stateFactory) { }


    public override void EnterState()
    {
        Debug.LogWarning("Player has entered LIGHT ATTACK 2 state");
        Ctx.DebugCurrentSubState = "Light Attack 2 State";

        //Ctx.IsFighting = true;
        Ctx.AttackType = 2;
        Ctx.IsLightAttacking2 = true;
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
        Ctx.TargetSpeed = 0f;
        if (Ctx.IsLightAttackPressed)
        {
            Ctx.IsComboAttacking = true;
            Ctx.CanComboAttack = false;
            Ctx.Animator.SetBool(Ctx.AnimIDLightAttack3, true);
        }
    }

    public override void ExitState()
    {
        Debug.Log("Player has exited LIGHT ATTACK 2 state");

        //Ctx.OnLightAttack2?.Invoke(true);
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
    }

    public override void InitializeSubStates()
    {

    }
}
