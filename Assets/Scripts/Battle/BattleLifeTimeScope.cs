using UnityEngine;
using VContainer.Unity;
using VContainer;
using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Enemies;
using Assets.Scripts.Enemies;
using Assets.Scripts.Battle;

public class BattleLifeTimeScope : LifetimeScope
{
    [SerializeField] private BattleManager _battleManager;
    [SerializeField] private BattleView _battleView;
    [SerializeField] private EnemyController _enemyController;
    [SerializeField] private EnemySpawner _enemySpawner;
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(_battleManager);
        builder.RegisterComponent(_enemyController);
        builder.RegisterComponent(_battleView);
        builder.RegisterComponent(_enemySpawner);

        builder.Register<IEnemyPoolManager,EnemyPoolManager>(Lifetime.Singleton);
        builder.Register<EnemyFactory>(Lifetime.Singleton);
    }

}
