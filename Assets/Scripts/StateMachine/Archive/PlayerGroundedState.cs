using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext, playerStateFactory) 
    {
        IsRootState = true;
        InitializeSubStates();
    }

    public override void EnterState() 
    {
        //Debug.LogWarning("Player has entered GROUNDED state");

        Ctx.OnGrounded?.Invoke(true);
        Ctx.DebugCurrentSuperState = "Grounded State";
    }

    public override void UpdateState() 
    {
       // Debug.Log("GROUNDED state is currently active");

        CheckSwitchStates();
        Ctx.EnemyDetection();
        Ctx.ThirdPersonMovement();
        if (Ctx.VerticalVelocity < 0.0f)
        {
            Ctx.VerticalVelocity = -2f;
        }
    }

    public override void ExitState() 
    {
         //Debug.LogWarning("Player has exited GROUNDED state");

        Ctx.OnGrounded?.Invoke(false);
    }

    public override void CheckSwitchStates() 
    {
        if (!Ctx.IsGrounded)
        {
            SwitchState(Factory.Fall());
        }
        if(Ctx.IsAttacking)
        {
            SwitchState(Factory.Fight());
        }
        
    }

    public override void InitializeSubStates() 
    {
        if(Ctx.MoveInput != Vector2.zero)
        {
            if (Ctx.IsSprintPressed)
            {
                SetSubState(Factory.Sprint());
            }
            else
            {
                SetSubState(Factory.Run());
            }
        }
        else
        {
            SetSubState(Factory.Idle());
        }   
    }
}
