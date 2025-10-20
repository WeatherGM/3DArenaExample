using Assets.Scripts.Core.Enums;
using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Stats;
using Assets.Scripts.Player;
using System.Collections.Generic;
using VContainer;

namespace Assets.Scripts.Enemies
{
    public class EnemyRuntimeStats : IRuntimeStats
    {
        private readonly Dictionary<StatsType, Stat> _stats = new();

        public void InitializeData(SO_EnemyStats stats)
        {
            _stats.Clear();

            _stats.Add(StatsType.HitPoints, new Stat(stats.HitPoints, StatsType.HitPoints));
            _stats.Add(StatsType.Damage, new Stat(stats.Damage, StatsType.Damage));
            _stats.Add(StatsType.AttackRadius, new Stat(stats.AttackRadius, StatsType.AttackRadius));
            _stats.Add(StatsType.AttackSpeed, new Stat(stats.AttackSpeed, StatsType.AttackSpeed));
            _stats.Add(StatsType.Speed, new Stat(stats.Speed, StatsType.Speed));

            _stats.Add(StatsType.MaxMemberCount, new Stat(stats.MaxMemberCount, StatsType.MaxMemberCount));
            _stats.Add(StatsType.SpawnWeight, new Stat(stats.SpawnWeight, StatsType.SpawnWeight));
            _stats.Add(StatsType.SpawnRadius, new Stat(stats.SpawnRadius, StatsType.SpawnRadius));
            _stats.Add(StatsType.SpawnChance, new Stat(stats.SpawnChance, StatsType.SpawnChance));
            _stats.Add(StatsType.StoppingDistance, new Stat(stats.StoppingDistance, StatsType.StoppingDistance));
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
