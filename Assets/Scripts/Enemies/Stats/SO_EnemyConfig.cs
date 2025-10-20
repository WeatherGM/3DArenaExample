using Assets.Scripts.Core.Enums;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    [CreateAssetMenu(fileName = "SO_EnemyConfig", menuName = "Configs/SO_EnemyConfig", order = 2)]
    public class SO_EnemyConfig : ScriptableObject
    {
        [field: SerializeField] public GameObject Prefab { get; private set; }

        [field: SerializeField] public EnemyType Type { get; private set; }

        [field: SerializeField] public SO_EnemyStats Stats { get; private set; }
    }
}
