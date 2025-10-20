using Assets.Scripts.Core.Interfaces;
using UnityEngine;
using VContainer;
namespace Assets.Scripts.Player
{
    public class PlayerController : MonoBehaviour,IDataInitializer
    {
        private PlayerInput _playerInput;

        private PlayerEntity _player;
        private PlayerCameraController _cameraController;
        private PlayerAnimatorController _animatorController;
        private IGameInvoker _battleInvoker;

        [Inject]
        public void Initialize(PlayerInput playerInput, 
            PlayerEntity player, 
            PlayerCameraController cameraController,
            PlayerAnimatorController animatorController,
            IGameInvoker battleInvoker)
        {
            _playerInput = playerInput;
            _cameraController = cameraController;
            _animatorController = animatorController;
            _battleInvoker = battleInvoker;
            _player = player;
        }

        public void InitializeData()
        {
            _player.OnJump +=  _animatorController.Jump;
            _player.OnAttack +=  _animatorController.Attack;
            _player.OnDeath += _animatorController.Die;
            _player.OnDeath += _battleInvoker.WaveLose;
        }

        void Update()
        {
            if (_player.IsLive == false)
                return;

            if (_player.IsAttack == false && _playerInput.HandleAttack())
                _player.Attack();

            if (_playerInput.HandleJump())
                _player.Jump();

            Vector3 move = _playerInput.HandleMovement();
            if (move != Vector3.zero && _player.IsAttack == false)
                _animatorController.Move();

            _player.Move(move);

            if (_playerInput.HasInput() == false && _player.IsAttack == false)
                _animatorController.Idle();

            _cameraController.RotateCamera();
        }
    }
}