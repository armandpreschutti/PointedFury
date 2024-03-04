using System.Collections;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext, playerStateFactory)
    {
        IsRootState= true;
        InitializeSubStates();
    }

    public override void EnterState()
    {
        Debug.LogWarning("Player has entered JUMP state");

        Ctx.OnJump?.Invoke(true);
        Ctx.JumpDurationDelta = Ctx.JumpDuration;
        Ctx.StartCoroutine(JumpTimeout());
        Ctx.IsJumpPressed = false;
    }

    public override void UpdateState() 
    {
        Debug.Log("JUMP state is currently active");
        Ctx.FreeMovement();
        HandleJump();
    }

    public override void ExitState()
    {
        Debug.LogWarning("Player has exited JUMP state");

        Ctx.OnJump?.Invoke(false);
    }

    public override void CheckSwitchStates() 
    {
        
    }

    public override void InitializeSubStates()
    {
        if (Ctx.MoveInput != Vector2.zero)
        {
            SetSubState(Factory.Run());
        }
        else
        {
            SetSubState(Factory.Idle());
        }
    }

    void HandleJump()
    {
        Ctx.JumpDurationDelta -= Time.deltaTime;

        Ctx.VerticalVelocity = Mathf.Sqrt(Ctx.JumpHeight * -2f * Ctx.Gravity);
        
    }

    IEnumerator JumpTimeout()
    {
        yield return new WaitForSeconds(Ctx.JumpDurationDelta);
        SwitchState(Factory.Fall());
    }

}
