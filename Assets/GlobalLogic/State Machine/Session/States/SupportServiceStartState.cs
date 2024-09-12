using Zenject;

public class SupportServiceStartState : IBaseState
{
    [Inject] private SessionStateMachine _statemachine;
    [Inject] private IMarkerService _markerService;
    [Inject] private ILoaderSceneService _loaderSceneService;
    [Inject] private IAudioService _audioService;

    public void Enter()
    {
        _markerService.ActivateService();
        _loaderSceneService.ActivateService();
        _audioService.ActivateService();

        _statemachine.SetState<SessionStartState>();
    }

    public void Exit()
    {

    }

    public void Update()
    {

    }
}
