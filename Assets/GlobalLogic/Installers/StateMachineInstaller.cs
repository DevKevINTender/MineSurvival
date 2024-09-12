using Zenject;

public class StateMachineInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<SessionStateMachine>().AsSingle();
        
        Container.Bind<SupportServiceStartState>().AsSingle();
        Container.Bind<SessionStartState>().AsSingle();
        Container.Bind<SessionEndState>().AsSingle();
    }
}