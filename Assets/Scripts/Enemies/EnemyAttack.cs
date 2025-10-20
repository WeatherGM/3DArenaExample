using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Stats;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class EnemyAttack : IEntityAttack
    {
        public Stat Damage { get; set; }

        private IDamageable _targetDamageable;
        private Transform _targetTr;

        public void SetAttackForward(Transform targetTr)
        {
            _targetTr = targetTr;
        }

        public void Attack(Vector3 attackOrigin, float attackRadius, int layerMask)
        {
            Vector3 direction = (_targetTr.position + Vector3.down - attackOrigin).normalized;

            if (Physics.Raycast(attackOrigin, direction, out RaycastHit hit, attackRadius, layerMask))
            {
                if (_targetDamageable == null && hit.collider.TryGetComponent(out IDamageable damageable))
                    _targetDamageable = damageable;

                if (_targetDamageable != null)
                    _targetDamageable.DealDamage(Damage.CurrentValue);
            }

            Debug.DrawRay(attackOrigin, direction * attackRadius, Color.red, 1f);
        }
    }
}
