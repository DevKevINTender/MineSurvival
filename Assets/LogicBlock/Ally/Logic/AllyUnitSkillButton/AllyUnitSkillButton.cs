using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using UniRx;
using TMPro;
using Zenject;

public class AllyUnitSkillButton: MonoBehaviour
{
    public Action OnActivateSkillAction;
    [SerializeField] private Button _activateSkill;
    [SerializeField] private Image _levelImage;
    [SerializeField] private Image _iconImage;
    [SerializeField] private List<Sprite> _levelList = new List<Sprite>();
    [SerializeField] private Image RecoveryFillAmount;
    [SerializeField] private GameObject recoveryObj;
    [SerializeField] private GameObject durationObj;
    [SerializeField] private Image skillStatus;
    [SerializeField] private TMP_Text recoverySkillText;
    [SerializeField] private TMP_Text durationSkillText;

    private ReactiveProperty<float> _currentSkillRecovery;
    private ReactiveProperty<float> _currentSkillDuration;
    private ReactiveProperty<SkillStatus> _currentStatus;
    private AllyUnitData _unitData;

    public void ActivateButton(
        ReactiveProperty<SkillStatus> status,
        AllyUnitData unitData,
        ReactiveProperty<float> currentSkillRecovery,
        ReactiveProperty<float> currentSkillDuration)
    {
        _unitData = unitData;
        _currentSkillRecovery = currentSkillRecovery;
        _currentSkillDuration = currentSkillDuration;
        _levelImage.sprite = _levelList[unitData.level];
        _iconImage.sprite = unitData.unitIcon;
        _iconImage.SetNativeSize();
        _activateSkill.onClick.AddListener(() => OnActivateSkillAction?.Invoke());

        _currentStatus = status;

        _currentSkillRecovery
            .Subscribe(x =>
            {
                RecoveryFillAmount.fillAmount = _currentSkillRecovery.Value / _unitData.skillRecovery;
                recoverySkillText.text = $"{_unitData.skillRecovery - _currentSkillRecovery.Value} сек";
            })
            .AddTo(this);

        _currentSkillDuration
            .Subscribe(x =>
            {
                durationSkillText.text = $"{_unitData.skillDuration - _currentSkillDuration.Value} сек";
            })
            .AddTo(this);

        _currentStatus
           .Subscribe(x =>
           {
               switch (x)
               {
                   case SkillStatus.Active:
                       skillStatus.color = new Color32(86, 86, 149, 255);
                       recoveryObj.SetActive(false);
                       durationObj.SetActive(true);
                       break;
                   case SkillStatus.Recovery:                  
                       skillStatus.color = new Color32(86, 86, 149, 255);
                       recoveryObj.SetActive(true);
                       durationObj.SetActive(false);
                       break;
                   case SkillStatus.ReadyToActive:
                       skillStatus.color = new Color32(255, 255, 255, 255);
                       recoveryObj.SetActive(false);
                       durationObj.SetActive(false);
                       break;
               }
           })
           .AddTo(this);
    }
}

public class AllyUnitSkillButtonService
{
    [Inject] private IViewFabric _viewFabric;
    [Inject] private IMarkerService _markerService;
    public ReactiveProperty<SkillStatus> _currentStatus = new();
    private AllyUnitSkillButton _allyUnitSkillButton;
    private ReactiveProperty<float> _currentSkillRecovery = new();
    private ReactiveProperty<float> _currentSkillDuration = new();
    private AllyUnitData _unitData;
    private CompositeDisposable _disposables = new();

    public void ActivateService(AllyUnitData unitData)
    {
        _unitData = unitData;
        Transform buttonPos = _markerService.GetTransformMarker<SkillButtonViewPanelMarker>();
        _allyUnitSkillButton = _viewFabric.Init<AllyUnitSkillButton>(buttonPos);
        _allyUnitSkillButton.ActivateButton(
            _currentStatus,
            _unitData,
            _currentSkillRecovery,
            _currentSkillDuration);
        _allyUnitSkillButton.OnActivateSkillAction = OnActivateSkillAction;

        RecoverySkill();
    }

    private void OnActivateSkillAction()
    {
        if (_currentStatus.Value != SkillStatus.ReadyToActive) return;

        _currentStatus.Value = SkillStatus.Active;

        Observable.Timer(System.TimeSpan.Zero, System.TimeSpan.FromSeconds(1))
            .Select(x => (int)x)
            .TakeWhile(x => x < _unitData.skillDuration)
            .Subscribe(x =>
            {
                _currentSkillDuration.Value = x;
            },
            () =>
            {
                RecoverySkill();
            })
            .AddTo(_disposables);
    }

    private void RecoverySkill()
    {
        _currentStatus.Value = SkillStatus.Recovery;
        Observable.Timer(System.TimeSpan.Zero, System.TimeSpan.FromSeconds(1))
            .Select(x => (int)x)
            .TakeWhile(x => x < _unitData.skillRecovery)
            .Subscribe(x =>
            {
                _currentSkillRecovery.Value = x;
            },
            () =>
            {
                _currentStatus.Value = SkillStatus.ReadyToActive;
            })
            .AddTo(_disposables);
    }

    public void DeactivateService()
    {
        _disposables.Dispose();
    }
}

public enum SkillStatus
{
    Active,
    Recovery,
    ReadyToActive
}