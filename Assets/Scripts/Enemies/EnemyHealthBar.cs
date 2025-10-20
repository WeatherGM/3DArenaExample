using Assets.Scripts.Core.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Enemies
{
    public class EnemyHealthBar : MonoBehaviour, IEntityHealthBar
    {
        [SerializeField] private TextMesh _text;
        public void ResetBar()
        {
            _text.text = "0/0";
            _text.gameObject.SetActive(false);
        }

        public void SetHP(float @base, float current)
        {
            _text.gameObject.SetActive(true);
            _text.text = $"{current}/{@base}";
        }
    }
}
