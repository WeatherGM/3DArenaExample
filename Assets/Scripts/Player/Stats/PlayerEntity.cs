using Assets.Scripts.Core;
using Assets.Scripts.Core.Base;
using Assets.Scripts.Core.Enums;
using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using VContainer;
namespace Assets.Scripts.Player
{
    public class PlayerEntity : Entity
    {
        public event Action OnMove;
        public event Action OnJump;
        public event Action OnAttack;
        public event Action OnDeath;

        [Header("Position Settings")]
        [SerializeField] private CharacterController _controller;
        [SerializeField] private Transform _playerTr;

        [Header("Movement Settings")]
        [SerializeField] private float _JumpHigh = 5f;
        [SerializeField] private float _gravity = -9.81f;
        [SerializeField] private float _rotateSpeed = 5f;

        [Header("Attack Settings")]
        [SerializeField] private Transform _attackOrigin;
        [SerializeField] private LayerMask _layerMaskBoids;

        private float _moveSpeed = 5f;
        private float _attackSpeed = 0.5f;
        private float _attackRadius = 1f;


        private Vector3 _velocity;
        private Vector3 _lastLookDirection = Vector3.forward;

        private bool _isGrounded;

        private Camera _camera;
        private SO_GameSave _save;
        private Coroutine _attackLoop;

        private const float ROTATE_THRESHOLD = 0.01f;

        [Inject]
        public void Initialize(IRuntimeStats runtimeStats,
            IEntityHealthBar healthBar,
            IEntityAttack entityAttack,
            IDamageProcessor damageProcessor,
            IDataService dataService)
        {
            RuntimeStats = runtimeStats;
            HealthBar = healthBar;
            AttackProcessor = entityAttack;
            DamageProcessor = damageProcessor;
            _save = dataService.GetGameSave();
            DamageProcessor.InitializeComponents(HealthBar, RuntimeStats.GetStat(StatsType.HitPoints));
        }

        public void Attack()
        {
            if (IsAttack == false && _attackLoop == null)
                _attackLoop = StartCoroutine(AttackRoutine());
        }

        public override void EndLiving()
        {
            IsLive = false;
            OnDeath?.Invoke();
        }

        public override void DealDamage(float damage)
        {
            if (IsLive == false)
                return;

            DamageProcessor.DealDamage(damage);
            if (RuntimeStats.GetStat(StatsType.HitPoints).CurrentValue <= 0)
                EndLiving();
        }
        public void Jump()
        {
            if (_isGrounded)
            {
                _velocity.y = Mathf.Sqrt(_JumpHigh * -2f * _gravity);
                OnJump.Invoke();
            }
        }

        public override void Move(Vector3 forward)
        {
            _isGrounded = _controller.isGrounded;

            Vector3 move = GetCameraDirection(forward);

            _controller.Move(move * _moveSpeed * Time.deltaTime);

            if (_isGrounded && _velocity.y < 0)
                _velocity.y = -2f;

             OnMove?.Invoke();

            UpdateRotate(move);

            _velocity.y += _gravity * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);
        }

        public override void StartLiving()
        {
            IsLive = true;
            _lastLookDirection = transform.forward;
            _camera = Camera.main;

            var hp = RuntimeStats.GetStat(StatsType.HitPoints);
            var damage = RuntimeStats.GetStat(StatsType.Damage);

            if(_save.LastHP != 0)
                hp.SetBase(_save.LastHP);

            if (_save.LastDamage != 0)
                damage.SetBase(_save.LastDamage);

            RuntimeStats.ResetAll();

            _attackSpeed = RuntimeStats.GetStat(StatsType.AttackSpeed).CurrentValue;
            _attackRadius = RuntimeStats.GetStat(StatsType.AttackRadius).CurrentValue;
            AttackProcessor.Damage = damage;

            _moveSpeed = RuntimeStats.GetStat(StatsType.Speed).CurrentValue;
            _JumpHigh = RuntimeStats.GetStat(StatsType.JumpHigh).CurrentValue;

            HealthBar.SetHP(
                hp.BaseValue,
                hp.CurrentValue);
        }

        private Vector3 GetCameraDirection(Vector3 inputVector)
        {
            Vector3 cameraForward = _camera.transform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();

            Vector3 cameraRight = _camera.transform.right;
            cameraRight.y = 0;
            cameraRight.Normalize();
            return cameraForward * inputVector.z + cameraRight * inputVector.x;
        }
        private void UpdateRotate(Vector3 lookDirection = default)
        {
            if (lookDirection != default)
                _lastLookDirection = lookDirection;

            if (_lastLookDirection.sqrMagnitude > ROTATE_THRESHOLD) 
            {
                _playerTr.rotation = Quaternion.Slerp(
                    _playerTr.rotation,
                    Quaternion.LookRotation(_lastLookDirection),
                    Time.deltaTime * _rotateSpeed);
            }

        }
        private Vector3 _lastAttackOrigin;
        private float _lastAttackRadius;
        private IEnumerator AttackRoutine()
        {
            IsAttack = true;        
            OnAttack?.Invoke();
            AttackProcessor.Attack(_attackOrigin.position, _attackRadius, _layerMaskBoids);
            _lastAttackOrigin = _attackOrigin.position;
            _lastAttackRadius = _attackRadius;
            Debug.Log("Атака");
            yield return new WaitForSeconds(_attackSpeed);

            IsAttack = false;
            _attackLoop = null;
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_lastAttackOrigin, _lastAttackRadius);
        }
    }
}