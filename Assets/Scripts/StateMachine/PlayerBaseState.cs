public abstract class PlayerBaseState 
{
    private bool _isRootState = false;
    private PlayerStateMachine _ctx;
    private PlayerStateFactory _factory;
    private PlayerBaseState _currentSuperState;
    private PlayerBaseState _currentSubState;

    protected bool IsRootState { set { _isRootState = value; } }
    protected PlayerStateMachine Ctx { get { return _ctx; } }
    protected PlayerStateFactory Factory { get { return _factory; } }

    public PlayerBaseState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
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
        if(_currentSubState!= null)
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
        if(_currentSubState!= null )
        {
            _currentSubState.ExitStates();
        }
    }

    protected void SwitchState(PlayerBaseState newState)
    {
        ExitStates();
        newState.EnterStates();
        if(_isRootState)
        {
            _ctx.CurrentState = newState;
        }
        else if(_currentSuperState!= null)
        {
            _currentSuperState.SetSubState(newState);
        }
    }
    
    protected void SetSuperState(PlayerBaseState newSuperSate) 
    {
        _currentSuperState = newSuperSate; 
    }

    protected void SetSubState(PlayerBaseState newSubState) 
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
}
