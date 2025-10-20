using Assets.Scripts.Core.Enums;

namespace Assets.Scripts.Core.Stats
{
    public class Stat
    {
        private float _baseValue;
        private float _currentValue;

        public StatsType StatsType { get; }
        public float BaseValue => _baseValue;
        public float CurrentValue => _currentValue;

        public Stat(float baseValue, StatsType statsType)
        {
            _baseValue = baseValue;
            _currentValue = baseValue;
            StatsType = statsType;
        }

        public void Set(float value)
        {
            _currentValue = value;
        }
        public void Reset()
        {
            _currentValue = _baseValue;
        }
        public void SetBase(float newBase)
        {
            _baseValue = newBase;
            _currentValue = newBase;
        }
    }
}