using Assets.Scripts.Core.Enemies;
using Assets.Scripts.Core.Enums;
using Assets.Scripts.Core.Interfaces;
using Lean.Pool;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class EnemyPoolManager : IEnemyPoolManager
    {
        private readonly IDataService _dataService;
        private readonly Dictionary<EnemyType, LeanGameObjectPool> _poolsPairs = new();
        public EnemyPoolManager(IDataService dataService)
        {
            _dataService = dataService;
        }

        public void InitializeData()
        {
            foreach (var cfg in _dataService.GetEnemiesConfigs())
                _poolsPairs.TryAdd(cfg.Type,CreatePool(cfg));

        }

        public EnemyEntity GetEnemyFromPool(EnemyType enemyType)
        {
            if (_poolsPairs.TryGetValue(enemyType, out var pool))
            {
                var go = pool.Spawn(null);
                if(go != null)
                    return go.GetComponent<EnemyEntity>();
            }
            return null;
        }
        public void ReturnToPool(EnemyEntity enemy)
        {
            if (_poolsPairs.TryGetValue(enemy.Type, out var pool))
                pool.Despawn(enemy.gameObject);
        }

        private LeanGameObjectPool CreatePool(SO_EnemyConfig config)
        {
            var type = config.Type;
            if (_poolsPairs.ContainsKey(type)) return null;

            var poolObject = new GameObject($"Pool_{type}");
            var pool = poolObject.AddComponent<LeanGameObjectPool>();
            pool.Prefab = config.Prefab;
            pool.Preload = config.Stats.MaxMemberCount;
            pool.Capacity = config.Stats.MaxMemberCount;
            pool.PreloadAll();

            return pool;
        }

        public bool IsAllDespawned()
        {
            bool allDespawned = true;
            foreach (var pool in _poolsPairs.Values)
                allDespawned = allDespawned && pool.Spawned == 0;
            return allDespawned;
        }

        public void DespawnAll()
        {
            foreach (var pool in _poolsPairs.Values)
                pool.DespawnAll();
        }
    }
}
