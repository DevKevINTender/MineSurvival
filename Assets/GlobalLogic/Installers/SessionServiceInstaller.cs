using Zenject;

public class SessionServiceInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        //DataManagers :
        //L1
        Container.Bind<ItemPoolDataManager>().AsSingle();
        Container.Bind<CoinDataManager>().AsSingle();
        Container.Bind<DailyGiftDataManager>().AsSingle();
        Container.Bind<EnemyListDataManager>().AsSingle();
        Container.Bind<DefaultLevelDataManager>().AsSingle();
        Container.Bind<AllyUnitDataManager>().AsSingle();
        //L2
        Container.Bind<UpgradeDataManager>().AsSingle();

        //Services :
        //L1
        Container.Bind<ShopPanelService>().AsSingle();
        Container.Bind<SessionScreenService>().AsSingle();
        Container.Bind<ShopItemPoolService>().AsSingle();
        Container.Bind<CoinPanelService>().AsSingle();
        Container.Bind<CoinGiftService>().AsSingle();
        Container.Bind<GemGiftService>().AsSingle();
        Container.Bind<TakeGiftScreenService>().AsSingle();
        Container.Bind<EnemySpawnService>().AsSingle();
        Container.Bind<SessionAllyUnitListService>().AsSingle();  
        //L2        
        Container.Bind<GiftServiceManager>().AsSingle();
        //L3
        Container.Bind<DailyGiftScreenService>().AsSingle();

    }
}
