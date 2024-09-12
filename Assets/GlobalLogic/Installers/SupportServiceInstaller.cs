using Zenject;

public class SupportServiceInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IEventService>().To<EventService>().AsSingle();
        Container.Bind<IMarkerService>().To<MarkerService>().AsSingle();

        Container.Bind<ILoaderSceneService>().To<LoaderSceneService>().AsSingle();

        Container.Bind(
            typeof(IPrefabStorageService),
            typeof(ISOStorageService)
            ).To(typeof(StorageService)).AsSingle();

        Container.Bind<IAudioDataManager>().To<AudioDataManager>().AsSingle();
        Container.Bind<IAudioService>().To<AudioService>().AsSingle();

        Container.Bind<IViewFabric>().To<ViewFabric>().AsSingle();
        Container.Bind<IServiceFabric>().To<ServiceFabric>().AsSingle();

        Container.Bind<IPoolsViewService>().To<PoolsViewService>().AsSingle();
    }
}
