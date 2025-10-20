using Assets.Scripts.Core;
using Assets.Scripts.Core.Enums;
using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Stats;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace Assets.Scripts.Player
{
    public class PlayerRuntimeStats : IRuntimeStats, IDataInitializer
    {
        private readonly IDataService _dataService;
        private readonly Dictionary<StatsType, Stat> _stats = new();

        [Inject]
        public PlayerRuntimeStats(IDataService dataService)
        {
            _dataService = dataService;
        }

        public void InitializeData()
        {
            var cfg = _dataService.GetPlayerStats();

            _stats.Clear();

            _stats.Add(StatsType.HitPoints, new Stat(cfg.HitPoints, StatsType.HitPoints));
            _stats.Add(StatsType.Damage, new Stat(cfg.Damage, StatsType.Damage));
            _stats.Add(StatsType.AttackRadius, new Stat(cfg.AttackRadius, StatsType.AttackRadius));
            _stats.Add(StatsType.AttackSpeed, new Stat(cfg.AttackSpeed, StatsType.AttackSpeed));
            _stats.Add(StatsType.Speed, new Stat(cfg.Speed, StatsType.Speed));
            _stats.Add(StatsType.JumpHigh, new Stat(cfg.JumpHigh, StatsType.JumpHigh));
        }


        public void ResetAll()
        {
            foreach (var s in _stats.Values)
                s.Reset();
        }

        public Stat GetStat(StatsType type)
        {
            return _stats.TryGetValue(type, out var stat) ? stat : null;
        }

    }
}
