public class PlayerStateFactory
{
    PlayerStateMachine _context;

    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        _context = currentContext;
    }

    public PlayerBaseState Fall()
    {
        return new PlayerFallState(_context, this);
    }

    public PlayerBaseState Idle() 
    {
        return new PlayerIdleState(_context, this);
    }
    public PlayerBaseState Run() 
    {
        return new PlayerRunState(_context, this);
    }
    public PlayerBaseState Sprint() 
    {
        return new PlayerSprintState(_context, this);
    }
    public PlayerBaseState Jump() 
    {
        return new PlayerJumpState(_context, this);
    }

    public PlayerBaseState Grounded() 
    {
        return new PlayerGroundedState(_context, this);
    }

    public PlayerBaseState Fight()
    {
        return new PlayerFightState(_context, this);
    }

    public PlayerBaseState LightAttack()
    {
        return new PlayerLightAttackSate(_context, this);
    }
}
