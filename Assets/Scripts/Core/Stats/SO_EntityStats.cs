using UnityEngine;
namespace Assets.Scripts.Core.Stats
{
    public class SO_EntityStats : ScriptableObject
    {
        [field: SerializeField] public float HitPoints { get; private set; } = 100;
        [field: SerializeField] public float Damage { get; private set; } = 5;
        [field: SerializeField] public float AttackRadius { get; private set; } = 5;
        [field: SerializeField] public float AttackSpeed { get; private set; } = 1;
        [field: SerializeField] public float Speed { get; private set; } = 8;
    }
}