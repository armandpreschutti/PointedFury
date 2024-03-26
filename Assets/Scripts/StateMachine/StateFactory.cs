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
    public BaseState LightAttack1()
    {
        return new LightAttack1Sate(_context, this);
    }
    public BaseState LightAttack2() 
    {
        return new LightAttack2State(_context, this);
    }
    public BaseState LightAttack3() 
    {
        return new LightAttack3State(_context, this);
    }
    public BaseState LightAttack4()
    {
        return new LightAttack4State(_context, this);
    }
    public BaseState LightAttack5()
    {
        return new LightAttack5State(_context, this);
    }
    public BaseState LightAttack6()
    {
        return new LightAttack6State(_context, this);
    }
    public BaseState LightAttack7()
    {
        return new LightAttack7State(_context, this);
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
