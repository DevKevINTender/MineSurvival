using Zenject;

public class CoinDataManager
{
    private CoinData CoinData;
    private const string key = "Coin";
    private ISOStorageService _storageService;
    private IEventService _eventService;

    [Inject]
    public CoinDataManager(ISOStorageService storageService, IEventService eventService)
    {
        _storageService = storageService;
        _eventService = eventService;
        CoinData = _storageService.GetSOByType<CoinData>();
        SaveLoader.LoadItem(key, CoinData);
    }

    public void DepositCoin(BigNumber depositCount)
    {
        CoinData.Count.Value += depositCount;
        _eventService.PublishEvent<OnDepositCoin>(new() {count = depositCount });
        SaveData();
    }

    public void DepositCoinWithoutEvent(BigNumber depositCount)
    {
        CoinData.Count.Value += depositCount;
        SaveData();
    }

    public bool WithdrawCoin(BigNumber withdrawCount)
    {
        if (withdrawCount <= CoinData.Count.Value)
        {
            CoinData.Count.Value -= withdrawCount;
            SaveData();
            return true;
        }
        return false;
    }

    public CoinData GetCoinData() => CoinData;

    private void SaveData()
    {
        SaveLoader.SaveItem(key, CoinData);
    }
}
