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
        return new LightAttackState(_context, this);
    }
    public BaseState Hurt()
    {
        return new HurtState(_context, this);
    }
    public BaseState Dash()
    {
        return new DashState(_context, this);
    }

    public BaseState Dodge()
    {
        return new DodgeState(_context, this);
    }

}
