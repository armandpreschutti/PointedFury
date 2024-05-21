public class StateFactory
{
    StateMachine _context;

    public StateFactory(StateMachine currentContext)
    {
        _context = currentContext;
    }

    public BaseState CombatState()
    {
        return new CombatState(_context, this);
    }

    public BaseState Idle()
    {
        return new IdleState(_context, this);
    }

    public BaseState Move()
    {
        return new MoveState(_context, this);
    }
    
    public BaseState FreeRoam()
    {
        return new FreeRoamState(_context, this);
    }
    
    public BaseState LightAttack()
    {
        return new LightAttackState(_context, this);
    }
    
    public BaseState HeavyAttack()
    {
        return new HeavyAttackState(_context, this);
    }
    
    public BaseState PostAttack()
    {
        return new PostAttackState(_context, this);
    }
    
    public BaseState Hurt()
    {
        return new HurtState(_context, this);
    }

    public BaseState Dash()
    {
        return new DashState(_context, this);
    }

    public BaseState Block()
    {
        return new BlockState(_context, this);
    }

    public BaseState Stunned()
    {
        return new StunnedState(_context, this);
    }

    public BaseState Parry()
    {
        return new ParryState(_context, this);
    }
    
    public BaseState Death()
    {
        return new DeathState(_context, this);
    }

    public BaseState Evade()
    {
        return new EvadeState(_context, this);
    }
}
