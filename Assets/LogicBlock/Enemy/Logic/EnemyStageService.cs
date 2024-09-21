using Zenject;
using System.Collections.Generic;
using System;
using UniRx;
using System.Diagnostics;
using UnityEngine;

public class EnemyStageService
{

    [Inject] private DefaultLevelDataManager _defaultLevelDataManager;
    [Inject] private IViewServicePoolService _viewServicePoolService;
    [Inject] private IServiceFabric _serviceFabric;
    private IViewServicePool _enemyViewServicePool;
    private EnemyStageStateService _enemyStageStateService;

    private List<EnemyUnitViewService> _activeEnemys = new();
    private CompositeDisposable _disposables = new();

    private List<EnemyData> _enemyDataist;
    private List<EnemyStageData> _enemyStageList;

    private EnemyStageData _currentEnemyStageData;
    private int _currentEnemyStageDataOrderNumber = 0;


    public void ActivateService()
	{
        _enemyStageList = _defaultLevelDataManager.GetEnemyStageDataList();


        _enemyViewServicePool = _viewServicePoolService.GetPool<EnemyUnitViewService>();

        _enemyStageStateService = _serviceFabric.InitSingle<EnemyStageStateService>();
        _enemyStageStateService.ActivateService();
        _enemyStageStateService.OnSpawnDelayAction += SpawnEnemyAction;
        _enemyStageStateService.OnCompleteRecoveryDelayAction += SetCurrentEnemyStageData;
        SetCurrentEnemyStageData();
    }

    private void SetCurrentEnemyStageData()
    {
        int orderNumber = _currentEnemyStageDataOrderNumber;
        if (orderNumber < _enemyStageList.Count)
        {
            _currentEnemyStageData = _enemyStageList[orderNumber];
            _enemyStageStateService.SetCurrentEnemyStage(_currentEnemyStageData);
            _currentEnemyStageDataOrderNumber++;
        }
        else
        {
            //Finish Session Event
            MonoBehaviour.print("SessionCompleted");
        }

    }

    private void SpawnEnemyAction()
    {
        var enemy = _enemyViewServicePool.GetItem() as EnemyUnitViewService;
        enemy.ActivateService(_currentEnemyStageData.EnemyName);
        _activeEnemys.Add(enemy);

    }

    public void DeactivateService()
    {
        _enemyStageStateService.OnSpawnDelayAction -= SpawnEnemyAction;
        _enemyStageStateService.DeactivateService();
        _disposables.Dispose(); 
    }
}



public class EnemyStageStateService
{
    public Action OnSpawnDelayAction;
    public Action OnCompleteRecoveryDelayAction;

    private ReactiveProperty<EnemyStageState> currentEnemyStageState = new();
    private IDisposable _spawnDelay;
    private IDisposable _recoveryDelay;
    private CompositeDisposable _disposables = new();

    private float _spawnDelayValue = 1f;
    private float _recoveryDelayValue = 5f;

    public ReactiveProperty<float> CurrentSpawnDelayValue = new();
    public ReactiveProperty<float> CurrentRecoveryDelayValue = new();

    private int _maxEnemySetOrderNumber = 0;
    private int _currentEnemySetOrderNumber = 0;
    private EnemyStageData _currentEnemyStageData;

    public void ActivateService()
    {
        currentEnemyStageState
           .Subscribe((x) =>
           {
               switch (x)
               {
                   case EnemyStageState.Recovery:
                       StopSpawnDelay();
                       StartRecoveryDelay();
                       break;
                   case EnemyStageState.Spawn:
                       StopRecoveryDelay();
                       StartSpawnDelay();
                       break;
                   case EnemyStageState.Wait:
                       StopRecoveryDelay();
                       StopSpawnDelay();
                       break;
               }
           })
           .AddTo(_disposables);
    }

    public void SetCurrentEnemyStage(EnemyStageData stageData)
    {
        _currentEnemySetOrderNumber = 0;
        _currentEnemyStageData = stageData;
        _maxEnemySetOrderNumber = stageData.Count;
        currentEnemyStageState.Value = EnemyStageState.Spawn;
    }

    private void StartSpawnDelay()
    {
        CurrentSpawnDelayValue.Value = 0;
        _spawnDelay = Observable.Interval(TimeSpan.FromSeconds(0.1))
           .Subscribe(x =>
           {
               CurrentSpawnDelayValue.Value += 0.1f;
           })
           .AddTo(_disposables);
        _spawnDelay = Observable.Interval(TimeSpan.FromSeconds(_spawnDelayValue))
           .Subscribe(x =>
           {
               OnSpawnDelayAction?.Invoke();
               _currentEnemySetOrderNumber++;
               if(_currentEnemySetOrderNumber >= _maxEnemySetOrderNumber)
               {
                    currentEnemyStageState.Value = EnemyStageState.Recovery;
               }
           })
           .AddTo(_disposables);
    }

    private void StopSpawnDelay()
    {
        _spawnDelay?.Dispose();
    }

    private void StartRecoveryDelay()
    {

        CurrentRecoveryDelayValue.Value = 0;
        _recoveryDelay = Observable.Interval(TimeSpan.FromSeconds(0.1))
           .Subscribe(x =>
           {
               CurrentRecoveryDelayValue.Value += 0.1f;
           })
           .AddTo(_disposables);

        _recoveryDelay = Observable.Interval(TimeSpan.FromSeconds(_recoveryDelayValue))
           .Subscribe(x =>
           {
               currentEnemyStageState.Value = EnemyStageState.Wait;
               OnCompleteRecoveryDelayAction?.Invoke();
               
           })
           .AddTo(_disposables);
    }

    public void DeactivateService()
    {
        _disposables.Dispose();
        StopRecoveryDelay();
        StopSpawnDelay();
    }

    private void StopRecoveryDelay()
    {
        _recoveryDelay?.Dispose();
    }
    public enum EnemyStageState
    {
        Recovery,
        Spawn,
        Wait
    }
}

