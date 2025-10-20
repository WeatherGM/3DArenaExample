using VContainer;
using VContainer.Unity;
using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Battle;

namespace Assets.Scripts.Core
{
    public class GameLifeTimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<IDataService, DataService>(Lifetime.Singleton);
            builder.Register<IGameInvoker, IGameEvents, BattleEvents>(Lifetime.Singleton);

            builder.RegisterEntryPoint<GameEntry>(Lifetime.Singleton);
        }
    }
}