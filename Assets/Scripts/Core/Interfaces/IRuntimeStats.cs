using Assets.Scripts.Core.Enums;
using Assets.Scripts.Core.Stats;

namespace Assets.Scripts.Core.Interfaces
{
    public interface IRuntimeStats
    {
        public Stat GetStat(StatsType type);
        public void ResetAll();
    }
}