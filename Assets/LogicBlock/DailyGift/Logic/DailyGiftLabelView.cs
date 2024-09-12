using Zenject;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;

public class DailyGiftLabelView : MonoBehaviour
{
    public int id;
    [SerializeField] private Image giftIcon;
    [SerializeField] private TMP_Text giftCountText;
    [SerializeField] private TMP_Text giftInfoText;
    [SerializeField] private GameObject isTakedStatus;

    public void ActivateView(
        DailyGiftData dailyGiftData,
        ReactiveProperty<GiftData> nextGift,
        ReactiveProperty<bool> isTakeToday)
    {
        giftIcon.sprite = dailyGiftData.giftData.icon;
        giftCountText.text = $"+{dailyGiftData.giftData.count}";
        dailyGiftData.isTaked.Subscribe((x) =>
        {
            isTakedStatus.SetActive(x);
        });

        nextGift?.Subscribe((x) => {
            if (x)
            {
                if (x == dailyGiftData.giftData)
                {
                    giftInfoText.text = isTakeToday.Value ? "Завтра" : "Сегодня";
                }
                else
                {
                    giftInfoText.text = $"День: {id + 1}";
                }
            }
        });
    }
    // Add your fields and methods here
}

public class DailyGiftLabelService
{
    [Inject] private DailyGiftDataManager _dailyGiftDataManager;
    private DailyGiftLabelView _dailyGiftLabelView;
    private DailyGiftData _dailyGiftData;

    public void ActivateService(DailyGiftLabelView dailyGiftLabelView, DailyGiftData dailyGiftData)
    {

        _dailyGiftLabelView = dailyGiftLabelView;
        _dailyGiftData = dailyGiftData;
        _dailyGiftLabelView.ActivateView(
            _dailyGiftData,
            _dailyGiftDataManager.nextGift,
            _dailyGiftDataManager.isTakeToday);
    }

    public void DeactivateService()
    {
        // Add your deactivation logic here
    }
}