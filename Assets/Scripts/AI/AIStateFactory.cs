public class AIStateFactory
{
    AIBrain _context;

    public AIStateFactory(AIBrain currentContext)
    {
        _context = currentContext;
    }

    public AIAttackerRooteState AttackerRootState()
    {
        return new AIAttackerRooteState(_context, this);
    }

    public AIWatcherRooteState WatcherRootState()
    {
        return new AIWatcherRooteState(_context, this);
    }

    public AIBaseState Idle()
    {
        return new AIIDleState(_context, this);
    }

    public AIBaseState Hurt()
    {
        return new AIHurtState(_context, this);
    }
}
