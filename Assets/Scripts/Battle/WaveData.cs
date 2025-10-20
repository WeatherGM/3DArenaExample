using UnityEngine;

namespace Assets.Scripts.Battle
{
    public class WaveData
    {
        private int _waveValue;
        private int _waveIndex;

        public int WaveValue
        {
            get => _waveValue;
            set => _waveValue = Mathf.Max(0, value);
        }

        public int WaveIndex
        {
            get => _waveIndex;
            set => _waveIndex = Mathf.Max(0, value);
        }
    }
}
