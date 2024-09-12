using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using UniRx;
using TMPro;

public class AllyUnitSkillButtonView: MonoBehaviour
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
    private AllyUnitData _skillData;

    public void ActivateView(
        ReactiveProperty<SkillStatus> status,
        AllyUnitData skillData,
        ReactiveProperty<float> currentSkillRecovery,
        ReactiveProperty<float> currentSkillDuration)
    {
        _skillData = skillData;
        _currentSkillRecovery = currentSkillRecovery;
        _currentSkillDuration = currentSkillDuration;
        _levelImage.sprite = _levelList[skillData.level];
        _iconImage.sprite = skillData.unitTierIconArray[skillData.level - 1];
        _iconImage.SetNativeSize();
        _activateSkill.onClick.AddListener(() => OnActivateSkillAction?.Invoke());

        _currentStatus = status;

        _currentSkillRecovery
            .Subscribe(x =>
            {
                RecoveryFillAmount.fillAmount = _currentSkillRecovery.Value / _skillData.skillRecovery;
                recoverySkillText.text = $"{_skillData.skillRecovery - _currentSkillRecovery.Value} сек";
            })
            .AddTo(this);

        _currentSkillDuration
            .Subscribe(x =>
            {
                durationSkillText.text = $"{_skillData.skillDuration - _currentSkillDuration.Value} сек";
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

