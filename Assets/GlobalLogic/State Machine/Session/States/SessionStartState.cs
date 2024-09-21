using DG.Tweening;
using Zenject;

public class SessionStartState : IBaseState
{
    [Inject] private SessionStateMachine _statemachine;

    [Inject] private ShopPanelService _shopPanelService;
    [Inject] private SessionScreenService _sessionScreenService;
    [Inject] private ShopItemPoolService _shopItemPoolService;
    [Inject] private CoinPanelService _coinPanelService;
    [Inject] private TakeGiftScreenService _takeGiftScreenService;
    [Inject] private DailyGiftScreenService _dailyGiftScreenService;
    [Inject] private EnemyStageService _enemyStageService;
    [Inject] private AllyUnitSlotArrayViewService _allyUnitSlotArrayViewService;
    [Inject] private SessionAllyUnitListService _sessionAllyUnitService;

    public void Enter()
    {
        _sessionScreenService.ActivateService();
        //_shopPanelService.ActivateService();
        //_shopItemPoolService.ActivateService();
        //_coinPanelService.ActivateService();
        //_takeGiftScreenService.ActivateService();
        //_dailyGiftScreenService.ActivateService();
        _enemyStageService.ActivateService();
        _allyUnitSlotArrayViewService.ActivateService();
        _sessionAllyUnitService.ActivateService();
    }

    public void Exit()
    {
        DOTween.Clear();
        LoaderSceneService.Instance.SetBufScene(0);
    }

    public void Update()
    {

    }
    public void ResetProgress()
    {
        _statemachine.SetState<SessionEndState>();
    }
}
