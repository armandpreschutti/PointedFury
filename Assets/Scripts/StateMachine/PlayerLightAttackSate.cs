using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerLightAttackSate : PlayerBaseState
{
    public PlayerLightAttackSate(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
     : base(currentContext, playerStateFactory) { }


    public override void EnterState()
    {
        Debug.LogWarning("Player has entered LIGHT ATTACK state");

        Ctx.OnLightAttack?.Invoke(true);
        Ctx.MoveTowardTarget(1f);
        Ctx.attackMoveTween.onComplete = () => { ExitState(); };
        Ctx.IsAttacking = true;
        Ctx.IsLightAttackPressed = false;
    }

    public override void UpdateState()
    {
        Ctx.TargetSpeed = 0f;
        Debug.Log("LIGHT ATTACK state is currently active");
        //Ctx.attackSequence.onComplete = () => { ExitState(); };
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Debug.LogError("Player has exited LIGHT ATTACK state");
        Ctx.OnLightAttack?.Invoke(false);
        Ctx.IsAttacking = false;
        
    }

    public override void CheckSwitchStates()
    {
        if(!Ctx.IsAttacking )
        {
            if (Ctx.MoveInput != Vector2.zero)
            {
                SwitchState(Factory.FightStrafe());
            }
            else
            {
                SwitchState(Factory.FightIdle());
            }
        }
        
       
    }

    public override void InitializeSubStates()
    {

    }
    
}
