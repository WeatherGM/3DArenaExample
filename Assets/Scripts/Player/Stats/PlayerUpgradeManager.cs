
using Assets.Scripts.Core;
using Assets.Scripts.Core.Enums;
using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Stats;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace Assets.Scripts.Player
{ 
    public class PlayerUpgradeManager : MonoBehaviour,IDataInitializer
    {
        [SerializeField] private float _hpBuffValue = 10;
        [SerializeField] private float _attackBuffValue = 1;

        private IRuntimeStats _playerStats;
        private IGameInvoker _gameInvoker;
        private SO_GameSave _save;
        private Dictionary<StatsType, Action> _statsChangedHandlers = new();
        [Inject]
        public void Initialize(
            IRuntimeStats playerStats,
            PlayerUpgradeView playerUpgradeView,
            IEntityHealthBar entityHealthBar, 
            IDataService dataService,
            IGameInvoker gameInvoker)
        {
            _playerStats = playerStats;
            _gameInvoker = gameInvoker;


            playerUpgradeView.InitializeActions(
                () => AddStatBuff(StatsType.Damage, _attackBuffValue),
                () => AddStatBuff(StatsType.HitPoints, _hpBuffValue));

            playerUpgradeView.InitializeValues(_attackBuffValue, _hpBuffValue);

            _statsChangedHandlers.Add(StatsType.HitPoints,
                () =>
                {
                    var stat = _playerStats.GetStat(StatsType.HitPoints);
                    _save.LastHP = stat.BaseValue;
                    entityHealthBar.SetHP(stat.BaseValue,stat.CurrentValue);
                });

            _statsChangedHandlers.Add(StatsType.Damage,
                () =>
                {
                    var stat = _playerStats.GetStat(StatsType.Damage);
                    _save.LastDamage = stat.BaseValue;
                    playerUpgradeView.ShowAttack(stat.CurrentValue);
                });

            _save = dataService.GetGameSave();
        }
        public void InitializeData()
        {
            _statsChangedHandlers[StatsType.Damage].Invoke();
        }
        private void AddStatBuff(StatsType statType, float buffValue)
        {
            var stat = _playerStats.GetStat(statType);
            var baseValue = stat.BaseValue;
            stat.SetBase(baseValue + buffValue);
            stat.Reset();
            _gameInvoker.BonusChanged();
            if (_statsChangedHandlers.TryGetValue(statType, out Action handler))
                handler.Invoke();
        }
    }
}
