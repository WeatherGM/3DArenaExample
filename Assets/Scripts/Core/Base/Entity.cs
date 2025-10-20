using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Stats;
using UnityEngine;


namespace Assets.Scripts.Core.Base
{
    public abstract class Entity : MonoBehaviour, IEntity
    {
        protected IRuntimeStats RuntimeStats;
        protected IEntityHealthBar HealthBar;
        protected IEntityAttack AttackProcessor;
        protected IDamageProcessor DamageProcessor;

        public bool IsLive { get; protected set; } = true;
        public bool IsAttack { get; protected set; } = false;

        public abstract void Move(Vector3 forward);
        public abstract void StartLiving();
        public abstract void EndLiving();
        public abstract void DealDamage(float damage);
    }
}
