using Assets.Scripts.Core.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Player
{
    public class PlayerHealthBar : MonoBehaviour, IEntityHealthBar
    {
        [SerializeField] private Slider _bar;
        [SerializeField] private Text _text;

        public void ResetBar()
        {
            _bar.maxValue = 1;
            _bar.value = 1;
        }

        public void SetHP(float @base, float current)
        {
            _bar.maxValue = @base;
            _bar.value = current;
            _text.text = $"{(int)current}/{(int)@base}";
        }
    }
}
