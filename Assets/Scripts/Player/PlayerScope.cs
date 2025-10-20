using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Stats;
using Assets.Scripts.Core;
using VContainer;
using VContainer.Unity;

namespace Assets.Scripts.Player
{
    public class PlayerScope : LifetimeScope
    {
        private PlayerEntity _player;
        private PlayerController _playerController;
        private PlayerCameraController _cameraController;
        private PlayerAnimatorController _playerAnimator;
        private PlayerHealthBar _playerHealthBar;
        private PlayerUpgradeManager _playerUpgradeManager;
        private PlayerUpgradeView _playerUpgradeView;
        private void FindMonobehaviors()
        {
            _player = FindAnyObjectByType<PlayerEntity>();
            _playerController = _player.GetComponentInChildren<PlayerController>();
            _cameraController = FindAnyObjectByType<PlayerCameraController>();
            _playerAnimator = FindAnyObjectByType<PlayerAnimatorController>();
            _playerHealthBar = FindAnyObjectByType<PlayerHealthBar>();
            _playerUpgradeManager = FindAnyObjectByType<PlayerUpgradeManager>();
            _playerUpgradeView = FindAnyObjectByType<PlayerUpgradeView>();

        }
        protected override void Configure(IContainerBuilder builder)
        {
            FindMonobehaviors();

            builder.RegisterComponent(_player);
            builder.RegisterComponent(_playerAnimator);
            builder.RegisterComponent(_playerController);
            builder.RegisterComponent(_cameraController);
            builder.RegisterComponent(_playerUpgradeManager);
            builder.RegisterComponent(_playerUpgradeView);
            builder.Register<IRuntimeStats, PlayerRuntimeStats>(Lifetime.Singleton);
            builder.Register<IEntityAttack, PlayerAttack>(Lifetime.Singleton);
            builder.Register<IDamageProcessor, PlayerDamage>(Lifetime.Singleton);
            builder.RegisterComponent<IEntityHealthBar>(_playerHealthBar);
            builder.Register<PlayerInput>(Lifetime.Singleton);

            builder.RegisterEntryPoint<PlayerEntry>(Lifetime.Singleton);
        }
    }
}
