using Assets.Scripts.Core.Enums;
using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Enemies;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using VContainer;

namespace Assets.Scripts.Core.Enemies
{
    public class EnemyFactory : IDataInitializer
    {
        private Dictionary<EnemyType, SO_EnemyConfig> _enemiesConfigs;

        private Transform _target;
        private readonly IDataService _dataService;
        private readonly IEnemyPoolManager _enemyPoolManager;
        [Inject]
        public EnemyFactory(IDataService dataService,
            IEnemyPoolManager enemyPoolManager)
        {
            _dataService = dataService;
            _enemyPoolManager = enemyPoolManager;
        }
        public void InitializeData()
        {
            _enemiesConfigs = _dataService.GetEnemiesConfigs().ToDictionary(k => k.Type);
        }
        public void InitializeTarget(Transform target)
        {
            _target = target;
        }
        public EnemyEntity CreateEnemy(EnemyType type)
        {
            if (_enemiesConfigs.TryGetValue(type, out SO_EnemyConfig cfg))
                return BuildEnemy(cfg);
            return null;
        }
        private EnemyEntity BuildEnemy(SO_EnemyConfig cfg)
        {
            var enemy = _enemyPoolManager.GetEnemyFromPool(cfg.Type);

            if (enemy == null)
                return null;

            var entityHealthBar = enemy.GetComponent<EnemyHealthBar>();
            var animator = enemy.GetComponent<MeleeEnemyAnimator>();

            enemy.ClearEvents();
            enemy.OnAttack += animator.Attack;
            enemy.OnResetAnimator += animator.ResetAnimator;
            enemy.OnMove += animator.Move;
            enemy.OnDeath += animator.Die;

            var stats = new EnemyRuntimeStats();
            stats.InitializeData(cfg.Stats);

            var attack = new EnemyAttack();
            attack.SetAttackForward(_target);

            var damage = new EnemyDamage();
            damage.InitializeComponents(entityHealthBar, stats.GetStat(StatsType.HitPoints));


            enemy.InitializeComponents(stats, entityHealthBar, attack, damage);

            return enemy;
        }
    }
}