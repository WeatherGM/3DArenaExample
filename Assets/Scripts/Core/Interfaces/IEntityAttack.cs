

using Assets.Scripts.Core.Stats;
using UnityEngine;

namespace Assets.Scripts.Core.Interfaces
{
    public interface IEntityAttack
    {
        public Stat Damage { get; set; }
        void Attack(Vector3 attackOrigin, float attackRaduis, int layerMask);
    }
}
