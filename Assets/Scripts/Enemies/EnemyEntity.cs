using Assets.Scripts.Core.Base;
using Assets.Scripts.Core.Enums;
using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Stats;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

namespace Assets.Scripts.Core.Enemies
{
    public class EnemyEntity : Entity
    {
        public event Action OnMove;
        public event Action OnAttack;
        public event Action OnDeath;
        public event Action OnResetAnimator;
        public event Action<EnemyEntity> OnDespawn;

        [field: SerializeField] public EnemyType Type { get; private set; }

        [SerializeField] private LayerMask _layerMaskPlayer;
        [Header("Move Settings")]
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private LookAtConstraint _lookAtConstaintHp;
        public Transform BattleCenter { get; set; }
        public bool NearTarget { get; private set; } = false;
        public int CurrentWave { get; set; } = 0;
        private Transform _target;

        private float _attackSpeed = 0.5f;
        private float _attackRadius = 1f;
        private float _spawnRadius;

        private Coroutine _attackLoop;
        private Coroutine _dieLoop;

        private readonly WaitForSeconds _timer = new(1.2f);

        private const float ATTACK_CORRECTOR = 10;
        public void InitializeComponents(
            IRuntimeStats runtimeStats,
            IEntityHealthBar entityHealthBar,
            IEntityAttack entityAttack,
            IDamageProcessor damageProcessor)
        {
            RuntimeStats = runtimeStats;
            HealthBar = entityHealthBar;
            AttackProcessor = entityAttack;
            DamageProcessor = damageProcessor;

            DamageProcessor.InitializeComponents(HealthBar, RuntimeStats.GetStat(StatsType.HitPoints));

            _target = Camera.main.transform;
        }

        public void Attack()
        {
            if (IsAttack == false && _attackLoop == null)
                _attackLoop = StartCoroutine(AttackRoutine());
        }
        public override void DealDamage(float damage)
        {
            if (IsLive == false)
                return;

            DamageProcessor.DealDamage(damage);
            if (RuntimeStats.GetStat(StatsType.HitPoints).CurrentValue <= 0)
                EndLiving();
        }

        public override void EndLiving()
        {
            if (_dieLoop == null)
                _dieLoop = StartCoroutine(DieRoutine());
        }

        public override void Move(Vector3 position)
        {
            if (_agent.pathPending)
                return;

            NearTarget = Vector3.Distance(transform.position, position) <= _attackRadius;

            if (IsAttack == false)
                OnMove.Invoke();

            _agent.isStopped = false;
            _agent.destination = position;
        }

        public override void StartLiving()
        {
            IsLive = true;
            _agent.enabled = true;
            var hpStat = RuntimeStats.GetStat(StatsType.HitPoints);
            hpStat.SetBase(hpStat.BaseValue + CurrentWave);

            var damageStat = RuntimeStats.GetStat(StatsType.Damage);
            damageStat.SetBase(damageStat.BaseValue + CurrentWave / ATTACK_CORRECTOR);

            _attackSpeed = RuntimeStats.GetStat(StatsType.AttackSpeed).CurrentValue;
            _attackRadius = RuntimeStats.GetStat(StatsType.AttackRadius).CurrentValue;

            AttackProcessor.Damage = damageStat;

            _spawnRadius = RuntimeStats.GetStat(StatsType.SpawnRadius).CurrentValue;
            _agent.speed = RuntimeStats.GetStat(StatsType.Speed).CurrentValue;
            _agent.stoppingDistance = RuntimeStats.GetStat(StatsType.StoppingDistance).CurrentValue;

            HealthBar.SetHP(hpStat.BaseValue,hpStat.CurrentValue);
            SetupLookAt(_lookAtConstaintHp);
            SetPosition();
        }

        public void ClearEvents()
        {
            OnDespawn = null;
            OnMove = null;
            OnResetAnimator = null;
            OnAttack = null;
        }


        private IEnumerator AttackRoutine()
        {
            IsAttack = true;
            OnAttack?.Invoke();
            AttackProcessor.Attack(transform.position, _attackRadius, _layerMaskPlayer);
            yield return new WaitForSeconds(_attackSpeed);

            IsAttack = false;
            _attackLoop = null;
        }

        private IEnumerator DieRoutine()
        {
            IsLive = false;
            _agent.enabled = false;
            HealthBar.ResetBar();
            OnDeath?.Invoke();
            yield return _timer;
            OnResetAnimator.Invoke();
            yield return _timer;
            OnDespawn.Invoke(this);
            _dieLoop = null;

        }

        private void SetPosition()
        {
            Vector3 center = BattleCenter.position;

            Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * _spawnRadius;
            Vector3 randomPoint = center + new Vector3(randomCircle.x, 0f, randomCircle.y);

            if (!NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, _spawnRadius, NavMesh.AllAreas))
                    return;
            
            if (!_agent.enabled)
                _agent.enabled = true;

            _agent.Warp(hit.position);
        }

        private void SetupLookAt(LookAtConstraint lookAtConstraint)
        {
            var source = new ConstraintSource
            {
                sourceTransform = _target,
                weight = 1f
            };

            lookAtConstraint.AddSource(source);
            lookAtConstraint.rotationOffset = new Vector3(0, 180, 0);
            lookAtConstraint.constraintActive = true;
        }
    }
}