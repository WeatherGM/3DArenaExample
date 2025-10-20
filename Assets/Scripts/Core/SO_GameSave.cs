using UnityEngine;

namespace Assets.Scripts.Core
{
    [CreateAssetMenu(fileName = "SO_Save", menuName = "Configs/GameSave", order = 4)]
    public class SO_GameSave : ScriptableObject
    {
        [field: SerializeField] public int LastWave { get; set; } = 0;
        [field: SerializeField] public float LastHP { get; set; } = 0;
        [field: SerializeField] public float LastDamage { get; set; } = 0;
    }
}
