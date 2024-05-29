using UnityEngine;

public class AIApproachingState: AIBaseState
{
    public AIApproachingState(AIBrain currentContext, AIStateFactory stateFactory)
    : base(currentContext, stateFactory) { }

   // float stateTime;
    //float _targetMoveInput = 1f;
    //float _moveInputSmoothTime = 30f;
    public override void EnterState()
    {
        //Debug.LogWarning("Enemy has entered APPROACHING state");
        //_targetMoveInput = 0;
    }

    public override void UpdateState()
    {
        // Debug.Log("APPROACHING state is currently active");
        Ctx.DebugSubState = "Approaching State";
        CheckSwitchStates();
        Ctx.timeSinceAttack += Time.deltaTime;
        // stateTime += Time.deltaTime;
        // Ctx.moveInput.y = Mathf.Lerp(Ctx.moveInput.y, _targetMoveInput, Time.deltaTime * _moveInputSmoothTime);
        /*if (Ctx.StateMachine.MoveInput.magnitude < 0.01f)
        {
            Ctx.StateMachine.MoveInput = Vector2.zero;
        }*/
        Ctx.moveInput.y = 1f;
    }

    public override void ExitState()
    {
        //Debug.LogWarning("Enemy has exited APPROACHING state");

    }

    public override void CheckSwitchStates()
    {
        if (Ctx.DistanceToPlayer(Ctx._currentTarget.transform) <= (Ctx.targetDistance))
        {
            SwitchState(Factory.Idle());
        }
      /*  else if (Ctx.isHurt)
        {
            SwitchState(Factory.Hurt());
        }*/
    }

    public override void InitializeSubStates()
    {

    }
}
