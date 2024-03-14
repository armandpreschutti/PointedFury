public class StateFactory
{
    StateMachine _context;

    public StateFactory(StateMachine currentContext)
    {
        _context = currentContext;
    }

    public BaseState FreeRoam()
    {
        return new FreeRoamState(_context, this);
    }

    public BaseState Idle()
    {
        return new IdleState(_context, this);
    }

    public BaseState Move()
    {
        return new MoveState(_context, this);
    }

    public BaseState Fight()
    {
        return new FightState(_context, this);
    }

    public BaseState LightAttack()
    {
        return new LightAttackSate(_context, this);
    }
    /*public PlayerBaseState Run()
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

    public PlayerBaseState FightIdle()
    {
        return new PlayerFightIdleState(_context, this);
    }

    public PlayerBaseState FightStrafe()
    {
        return new PlayerFightStrafeState(_context, this);
    }

    public PlayerBaseState LightAttack()
    {
        return new PlayerLightAttackSate(_context, this);
    }*/
}
