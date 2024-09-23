using System;
using UniRx;

public class MoveService
{
    public Action AchiveTargetAction;
    public Action LooseTargetAcion;
    private TargetFinderComponent _targetFinder;
    private MoveComponent _moveComponent;
    private float _unitSpeed;
    private CompositeDisposable _disposables = new();

    public void ActivateService(
        TargetFinderComponent targetFinder,
        MoveComponent moveComponent,
        float speed,
        float stopDistance = 0)
    {
        _targetFinder = targetFinder;
        _moveComponent = moveComponent;
        _unitSpeed = speed;
        _moveComponent.AchiveTargetAction += AchiveTarget;
        _moveComponent.LooseTargetAcion += LooseTarget;

        _targetFinder.CurrentTarget
            .Subscribe(x =>
            {
                if(x != null) _moveComponent.MoveToTarget(x, _unitSpeed, stopDistance);
            }).AddTo(_disposables);
       
    }

    private void AchiveTarget()
    {
        AchiveTargetAction?.Invoke();
    }

    private void LooseTarget()
    {
        LooseTargetAcion?.Invoke();
    }

    public void DeactivateService()
    {
        _moveComponent.AchiveTargetAction -= AchiveTarget;
        _moveComponent.LooseTargetAcion -= LooseTarget;
        _moveComponent.StopAllCoroutines();
        _disposables.Dispose();
    }
}



