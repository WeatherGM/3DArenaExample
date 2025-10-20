using Assets.Scripts.Core.Enums;
using Assets.Scripts.Core.Stats;
using Assets.Scripts.Enemies;
using Assets.Scripts.Player;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Interfaces
{
    public interface IDataService 
    {
        SO_StatsPlayer GetPlayerStats();

        List<SO_EnemyConfig> GetEnemiesConfigs();
        SO_GameSave GetGameSave();
    }
}