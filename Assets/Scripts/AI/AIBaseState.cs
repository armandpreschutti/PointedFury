public abstract class AIBaseState
{
    private bool _isRootState = false;
    private AIBrain _ctx;
    private AIStateFactory _factory;
    private AIBaseState _currentSuperState;
    private AIBaseState _currentSubState;

    protected bool IsRootState { set { _isRootState = value; } }
    protected AIBrain Ctx { get { return _ctx; } }
    protected AIStateFactory Factory { get { return _factory; } }

    public AIBaseState(AIBrain currentContext, AIStateFactory aiStateFactory)
    {
        _ctx = currentContext;
        _factory = aiStateFactory;
    }

    public abstract void EnterState();

    public abstract void UpdateState();

    public abstract void ExitState();

    public abstract void CheckSwitchStates();

    public abstract void InitializeSubStates();

    public void UpdateStates()
    {
        UpdateState();
        if (_currentSubState != null)
        {
            _currentSubState.UpdateStates();
        }
    }

    public void EnterStates()
    {
        EnterState();
        if (_currentSubState != null)
        {
            _currentSubState.EnterStates();
        }
    }

    public void ExitStates()
    {
        ExitState();
        if (_currentSubState != null)
        {
            _currentSubState.ExitStates();
        }
    }

    protected void SwitchState(AIBaseState newState)
    {
        ExitStates();
        newState.EnterStates();
        if (_isRootState)
        {
            _ctx.CurrentState = newState;
        }
        else if (_currentSuperState != null)
        {
            _currentSuperState.SetSubState(newState);
        }
    }

    protected void SetSuperState(AIBaseState newSuperSate)
    {
        _currentSuperState = newSuperSate;
    }

    protected void SetSubState(AIBaseState newSubState)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
}
