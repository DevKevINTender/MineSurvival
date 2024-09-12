using Zenject;
using System.Collections.Generic;
using System;
using UniRx;
using UnityEngine;

public class EnemySpawnService
{
    [Inject] private EnemyListDataManager _enemyListDataManager;
    [Inject] private DefaultLevelDataManager _defaultLevelDataManager;
    [Inject] private IServiceFabric _serviceFabric;
    [Inject] private IViewFabric _viewFabric;
    [Inject] private IMarkerService _markerService;

    private List<EnemyViewService> _activeEnemys = new();
    private List<IDisposable> _disposables = new();

    public void ActivateService()
	{
        LevelData currentLevel = _defaultLevelDataManager.GetCurrentLevelData();
        List<EnemyData> enemyDatas = _enemyListDataManager.GetEnemyListData().enemyDatas;
        Transform parent = _markerService.GetTransformMarker<EnemySpawnPosMarker>();
        int currentEnemySetId = 0;
        Observable.Interval(TimeSpan.FromSeconds(5))
            .Subscribe(second =>
            {
                EnemySetData EnemySetList = currentLevel.EnemySetList[currentEnemySetId];
                Vector3 spawGap = new Vector3(0.25f, 0);
                for (int i = 0; i < EnemySetList.count; i++)
                {
                    EnemyView enemyView = _viewFabric.Init<EnemyView>(enemyDatas[EnemySetList.enemyId].prefab, parent.position - spawGap * i);
                    EnemyViewService newEnemy = _serviceFabric.InitMultiple<EnemyViewService>();
                    newEnemy.ActivateService(enemyView);
                    _activeEnemys.Add(newEnemy);
                }
                
                if (currentLevel.EnemySetList.Count > currentEnemySetId + 1) currentEnemySetId++;
            })
            .AddTo(_disposables);
    }
}

public enum EnemyEnum
{

}


