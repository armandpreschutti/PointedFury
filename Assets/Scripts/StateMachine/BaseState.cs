public abstract class BaseState
{
    private bool _isRootState = false;
    private StateMachine _ctx;
    private StateFactory _factory;
    private BaseState _currentSuperState;
    private BaseState _currentSubState;

    protected bool IsRootState { set { _isRootState = value; } }
    protected StateMachine Ctx { get { return _ctx; } }
    protected StateFactory Factory { get { return _factory; } }

    public BaseState(StateMachine currentContext, StateFactory playerStateFactory)
    {
        _ctx = currentContext;
        _factory = playerStateFactory;
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

    protected void SwitchState(BaseState newState)
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

    protected void SetSuperState(BaseState newSuperSate)
    {
        _currentSuperState = newSuperSate;
    }

    protected void SetSubState(BaseState newSubState)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
}
