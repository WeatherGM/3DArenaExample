using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Stats;
using VContainer;
using VContainer.Unity;

namespace Assets.Scripts.Player
{
    public class PlayerEntry : IStartable
    {
        private PlayerEntity _player;
        private PlayerRuntimeStats _stats;
        private IEntityHealthBar _healthBar;
        private IDamageProcessor _damageProcessor;

        private IDataInitializer _playerController;
        private IDataInitializer _playerUpgradeManager;

        [Inject]
        public PlayerEntry(
            PlayerController playerController,
            PlayerEntity player,
            IEntityHealthBar helathBar,
            IRuntimeStats stats,
            IDamageProcessor damageProcessor,
            PlayerUpgradeManager playerUpgradeManager)
        {
            _playerController = playerController;
            _playerUpgradeManager = playerUpgradeManager;
            _player = player;
            _damageProcessor = damageProcessor;
            _stats = (PlayerRuntimeStats)stats;
            _healthBar = helathBar;
        }

        public void Start()
        {
            UnityEngine.Debug.Log("Start player");
            _playerController.InitializeData();
            _stats.InitializeData();
            _damageProcessor.InitializeComponents(_healthBar, _stats.GetStat(Core.Enums.StatsType.HitPoints));
            _player.StartLiving();
            _playerUpgradeManager.InitializeData();
        }
    }
}
