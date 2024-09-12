using Zenject;
using UnityEngine;
using System;

public class EnemyView : MonoBehaviour
{
    private Rigidbody2D rb;

    public void ActivateView()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector3.right;
    }

    public void DisableView()
    {
        gameObject.SetActive(false);
    }
}

public class EnemyViewService
{
	[Inject] private IViewFabric _fabric;
    [Inject] private IMarkerService _markerService;
	private EnemyView _enemyView;
    private HpComponent _hpComponent;
    private EnemyTakeDamage _enemyTakeDamage;


    public void ActivateService(EnemyView enemyView)
    {
        _enemyView = enemyView;
        _enemyView.ActivateView();

        _hpComponent = _enemyView.GetComponent<HpComponent>();
        _hpComponent.ActivateComponent(100);
        _hpComponent.DieAction += OnDieAction;

        _enemyTakeDamage = _enemyView.GetComponent<EnemyTakeDamage>();
        _enemyTakeDamage.ActivateComponent<AllyDealDamageComponent>();
    }

    public void OnDieAction()
    {
        DeactivateView();
    }

    public void DeactivateView()
    {
        _enemyView.DisableView();
    }
}



