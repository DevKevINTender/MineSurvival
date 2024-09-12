using UniRx;
using UnityEngine;
using Zenject;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;

public class TakeGiftScreenView: MonoBehaviour
{

    public Action OnCompleteTakeGiftAction;

    [SerializeField] private Image giftIcon;
    [SerializeField] private TMP_Text giftCountText;
    [SerializeField] private Animation anim;

    public void ActivateView(GiftData giftData)
    {
        giftIcon.sprite = giftData.icon;
        giftCountText.text = giftData.count > 0 ? $"+{giftData.count}" : "";
        gameObject.SetActive(true);

        anim.Play();
        Invoke("DeactivateView", anim.clip.length);
    }

    public void DeactivateView() 
    {
        anim.Stop();
        gameObject.SetActive(false);
        OnCompleteTakeGiftAction?.Invoke();
    }
}

public class TakeGiftScreenService : IService
{
    [Inject] private IViewFabric _viewFabric;
    [Inject] private IMarkerService _markerService;
    [Inject] private IEventService _eventService;

    private List<GiftData> giftDatas = new List<GiftData> ();
    private bool isActive = false;

    private TakeGiftScreenView _takeGiftScreenView;
    public void ActivateService()
    {
        _takeGiftScreenView = _viewFabric.Init<TakeGiftScreenView>();
        _takeGiftScreenView.DeactivateView();
        _takeGiftScreenView.OnCompleteTakeGiftAction = OnCompleteTakeGiftAction;
        _eventService.ObserveEvent<OnTakeGift>().Subscribe((x) => OnTakeGiftEvent(x.giftData));

    }

    public void OnTakeGiftEvent(GiftData giftData)
    {
        giftDatas.Add(giftData);
        if(isActive == false)
        {
            TakeGift();
        }
    }

    public void DeactivateService()
    {
    }

    public void TakeGift()
    {
        isActive = true;
       
        if (giftDatas.Count > 0)
        {
            GiftData giftData = giftDatas[0];
            giftDatas.Remove(giftData);
            _takeGiftScreenView.ActivateView(giftData);
        }
        else
        {
            isActive = false;
        }
    }

    public void OnCompleteTakeGiftAction()
    {
        isActive = false;
        TakeGift();
    }
}
