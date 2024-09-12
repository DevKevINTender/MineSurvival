using Zenject;

public class SessionEndState : IBaseState
{
    [Inject] private SessionStateMachine _statemachine;
    [Inject] private UpgradeDataManager _upgradeDataManager;
    [Inject] private ItemPoolDataManager _itemPoolDataManager;
    [Inject] private SessionAllyUnitListService _skillService;
  
    public void Enter()
    {
        _upgradeDataManager.Dispose();
        _itemPoolDataManager.Dispose();
        _skillService.DeactivateService();
        //LoaderSceneService.Instance.LoadBufScene();
    }

    public void Exit()
    {

    }

    public void Update()
    {

    }

   
}
