using Assets.Scripts.Core.Enums;
using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Stats;
using Assets.Scripts.Enemies;
using Assets.Scripts.Player;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

namespace Assets.Scripts.Core
{
    public class DataService : IDataService
    {
        private SO_StatsPlayer _playerConfig;
        private SO_GameSave _gameSave;

        [Inject]
        public void Initialize()
        {
            _playerConfig = Resources.Load<SO_StatsPlayer>("SO_Player");
            _gameSave = Resources.Load<SO_GameSave>("SO_Save");
        }
        public SO_StatsPlayer GetPlayerStats()
        {
            return _playerConfig;
        }

        public List<SO_EnemyConfig> GetEnemiesConfigs()
        {
            var array = Resources.LoadAll<SO_EnemyConfig>("Configs/");
            if (array != null)
                return array.ToList();
            return null;
        }
        public SO_GameSave GetGameSave()
        {
            return _gameSave;
        }
    }
}
