using Assets.Scripts.Battle;
using Assets.Scripts.Core.Enums;
using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;
namespace Assets.Scripts.Core.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        public event Action OnDie;
        public bool HasWeightValue { get; private set; } = false;
        public bool AllDespawn { get; private set; } = true;

        private bool _forcedExit = false;
        private WaveData _currentWave;

        private IEnemyPoolManager _enemyPool;
        private EnemyFactory _enemyFactory;
        private Dictionary<EnemyType,EnemySpawnData> _datas;
        private List<(EnemyType type, int weight)> _weights;
        [Inject]
        public void Initialize(IEnemyPoolManager enemyPool, 
            EnemyFactory enemyFactory, 
            IDataService dataService)
        {
            _enemyPool = enemyPool;
            _enemyFactory = enemyFactory;
            _datas = dataService.GetEnemiesConfigs().ToDictionary(k=>k.Type,v=>
                new EnemySpawnData() { 
                    Type = v.Type,
                    SpawnChance = v.Stats.SpawnChance,
                    SpawnWeight = v.Stats.SpawnWeight
                });
                

            _weights = _datas.Values.Select(d => (d.Type, d.SpawnWeight)).ToList();
        }
        public void InitWaveData(WaveData waveData)
        {
            _currentWave = waveData;
            HasWeightValue = true;
            _forcedExit = false;
        }
        public EnemyEntity Spawn()
        {
            if (_forcedExit)
                return null;

            var type = ProbabilityPicker.PickOne(_weights);

            if ((_currentWave.WaveValue -= _datas[type].SpawnWeight) >= 0)
            {
                var enemy = _enemyFactory.CreateEnemy(ProbabilityPicker.PickOne(_weights));

                if (enemy == null)
                    return null;

                enemy.OnDespawn -= Despawn;
                enemy.OnDeath -= Die;
                enemy.OnDeath += Die;
                enemy.OnDespawn += Despawn;
                AllDespawn = _enemyPool.IsAllDespawned();
                return enemy;
            }
            else
                HasWeightValue = false;

            AllDespawn = _enemyPool.IsAllDespawned();
            return null;
        }
        public void ForcedEnd()
        {
            HasWeightValue = false;
            AllDespawn = true;
            _forcedExit = true;
        }
        private void Despawn(EnemyEntity enemy) => _enemyPool.ReturnToPool(enemy);

        private void Die() => OnDie.Invoke();

    }
}