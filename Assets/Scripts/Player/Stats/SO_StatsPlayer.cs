using Assets.Scripts.Core.Stats;
using UnityEngine;

namespace Assets.Scripts.Player
{
    [CreateAssetMenu(fileName = "SO_Player", menuName = "Configs/PlayerStats", order = 0)]
    public class SO_StatsPlayer : SO_EntityStats
    {
        [field: SerializeField] public float JumpHigh { get; private set; } = 5;
    }
}
