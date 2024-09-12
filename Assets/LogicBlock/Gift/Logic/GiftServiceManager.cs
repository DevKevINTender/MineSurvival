using System.Collections.Generic;
using Zenject;

public class GiftServiceManager
{
    private Dictionary<GiftEnum, IGiftService> enumToServiceDic = new Dictionary<GiftEnum, IGiftService>();
    private IEventService _eventService;
    [Inject]
    public GiftServiceManager
    (
        IEventService eventService,
        CoinGiftService coinGiftService,
        GemGiftService gemGiftService
    ) 
    {
        _eventService = eventService;
        enumToServiceDic.Add(GiftEnum.Coin, coinGiftService);
        enumToServiceDic.Add(GiftEnum.Gem, gemGiftService);
    }

    public void TakeGift(GiftData giftData)
    {
        enumToServiceDic.GetValueOrDefault(giftData.type).TakeGift();
        _eventService.PublishEvent<OnTakeGift>(new() {giftData = giftData });
    }
}






