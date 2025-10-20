using Assets.Scripts.Core.Enums;
using Assets.Scripts.Core.Stats;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    [CreateAssetMenu(fileName = "SO_EnemyStats", menuName = "Configs/EnemyStats", order = 1)]
    public class SO_EnemyStats : SO_EntityStats
    {
        [field: SerializeField] public float StoppingDistance { get; private set; } = 1;
        [field: SerializeField, Range(1, 100)] public int MaxMemberCount { get; private set; } = 100;
        [field: SerializeField, Range(1, 15)] public float SpawnRadius { get; private set; } = 5;
        [field: SerializeField, Range(1, 3)] public int SpawnWeight { get; private set; } = 1;
        [field: SerializeField, Range(1, 100)] public float SpawnChance { get; private set; } = 10;
    }
}
