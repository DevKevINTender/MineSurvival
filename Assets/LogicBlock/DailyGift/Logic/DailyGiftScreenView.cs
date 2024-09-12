using Zenject;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using UniRx;

public class DailyGiftScreenView : MonoBehaviour
{


    public Action TakeCurrentGiftAction;
    public Action CloseScreenAction;

    public Button takeGiftBtn;
    public Button closeScreenBtn;

    public GameObject takeGiftBtnIsTakedStatus;

    public ButtonAnimation takeGiftBtnAnim;
    public ButtonAnimation closeScreenBtnAnim;

    public List<DailyGiftLabelView> dailyGiftLabelViews = new List<DailyGiftLabelView>();

    public void ActivateView(ReactiveProperty<bool> isTakeToday)
    {
        takeGiftBtn.onClick.AddListener(() => TakeCurrentGiftAction?.Invoke());
        closeScreenBtn.onClick.AddListener(() => CloseScreenAction?.Invoke());

        isTakeToday.Subscribe(x => {
            takeGiftBtnIsTakedStatus.SetActive(x);
        });
    }

    public void HideView()
    {
        gameObject.SetActive(false);
    }
}


public class DailyGiftScreenService : IService
{
    [Inject] private IViewFabric _viewFabric;
    [Inject] private IServiceFabric _serviceFabric;
    [Inject] private IMarkerService _markerService;
    [Inject] private GiftServiceManager _giftServiceManager;
    [Inject] private DailyGiftDataManager _dailyGiftDataManager;
    private DailyGiftScreenView _dailyGiftPanelView;
    private List<DailyGiftLabelService> _dailyGiftLabelServices = new List<DailyGiftLabelService>();

    public void ActivateService()
    {
        _dailyGiftPanelView = _viewFabric.Init<DailyGiftScreenView>();
        _dailyGiftPanelView.TakeCurrentGiftAction = TakeCurrentGiftAction;
        _dailyGiftPanelView.CloseScreenAction = CloseScreenAction;
        _dailyGiftPanelView.ActivateView(_dailyGiftDataManager.isTakeToday);

        foreach (var itemView in _dailyGiftPanelView.dailyGiftLabelViews)
        {
            DailyGiftLabelService dailyGiftLabelService = _serviceFabric.InitMultiple<DailyGiftLabelService>();
            DailyGiftData dailyGiftData = _dailyGiftDataManager.GetDailyGiftDataById(itemView.id);
            dailyGiftLabelService.ActivateService(itemView, dailyGiftData);
            _dailyGiftLabelServices.Add(dailyGiftLabelService);
        }

    }

    public void TakeCurrentGiftAction()
    {
        if(_dailyGiftDataManager.isTakeToday.Value)
        {
            _dailyGiftPanelView.takeGiftBtnAnim.NegativeAnim();
        }
        else
        {
            GiftData currentGift = _dailyGiftDataManager.TakeCurrentGift();
            if(currentGift)
            {
                _giftServiceManager.TakeGift(currentGift);
                _dailyGiftPanelView.takeGiftBtnAnim.PositiveAnim();
            }
            else
            {
                _dailyGiftPanelView.takeGiftBtnAnim.NegativeAnim();
            }
        }
    }

    public void CloseScreenAction()
    {

        _dailyGiftPanelView.closeScreenBtnAnim.PositiveAnim(() => _dailyGiftPanelView.HideView());
    }

    public void DeactivateService()
    {
        // Add your deactivation logic here
    }
}
