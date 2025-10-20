using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Stats;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerAttack : IEntityAttack
    {
        public Stat Damage { get; set; }


        private Dictionary<Collider, IDamageable> _cashedColiders = new();

        private readonly Collider[] _entityColiders = new Collider[10];

        public void Attack(Vector3 attackOrigin, float attackRaduis,int layerMask)
        {
            int count = Physics.OverlapSphereNonAlloc(attackOrigin, attackRaduis, _entityColiders, layerMask, QueryTriggerInteraction.Collide);
            for (int i = 0; i < count; i++)
            {
                if (_cashedColiders.TryGetValue(_entityColiders[i], out IDamageable enemy))
                {
                    enemy.DealDamage(Damage.CurrentValue);
                    return;
                }

                if (_entityColiders[i].TryGetComponent(out IDamageable enemy1))
                    enemy1.DealDamage(Damage.CurrentValue);
            }
        }
    }
}
