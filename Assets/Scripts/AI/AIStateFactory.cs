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

    public AIBaseState Approaching()
    {
        return new AIApproachingState(_context, this);
    }

    public AIBaseState Disengaging()
    {
        return new AIDisengagingState(_context, this);
    }

    public AIBaseState Strafing()
    {
        return new AIStrafingState(_context, this);
    }

    public AIBaseState Attack()
    {
        return new AIAttackState(_context, this);   
    }

    public AIBaseState Block()
    {
        return new AIBlockState(_context, this);
    }
}
