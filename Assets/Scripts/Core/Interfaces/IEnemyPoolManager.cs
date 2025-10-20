using Assets.Scripts.Core.Enemies;
using Assets.Scripts.Core.Enums;

namespace Assets.Scripts.Core.Interfaces
{
    public interface IEnemyPoolManager : IDataInitializer
    {
        EnemyEntity GetEnemyFromPool(EnemyType enemyType);
        void ReturnToPool(EnemyEntity enemy);

        void DespawnAll();
        bool IsAllDespawned();
    }
}