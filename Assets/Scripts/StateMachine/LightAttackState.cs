using UnityEngine;

public class LightAttackSate : BaseState
{
    public LightAttackSate(StateMachine currentContext, StateFactory stateFactory)
    : base(currentContext, stateFactory) { }


    public override void EnterState()
    {
        Debug.LogWarning("Player has entered LIGHT ATTACK state");
        Ctx.DebugCurrentSubState = "Light Attack State";

        Ctx.OnAttack?.Invoke(true);
        Ctx.IsAttacking = true;
        Ctx.IsLightAttackPressed = false;
    }

    public override void UpdateState()
    {
        //Debug.Log("LIGHT ATTACK state is currently active");
        Ctx.RotateTowardTarget(.01f);
        Ctx.TargetSpeed = 0f;
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        //Debug.Log("Player has exited LIGHT ATTACK state");

        Ctx.SetAttackType();
        Ctx.OnAttack?.Invoke(false);
        Ctx.FightTimeoutActive = true;
        Ctx.FIghtTimeoutDelta = Ctx.AttackTimeout;
        Ctx.IsAttacking = false;
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsAttacking)
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

    public override void InitializeSubStates()
    {

    }
}
