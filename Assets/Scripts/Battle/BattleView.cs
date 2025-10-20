using Assets.Scripts.Core.Interfaces;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Assets.Scripts.Battle
{
    public class BattleView : MonoBehaviour
    {
        [SerializeField] private Text _timerBeforeStart;
        [SerializeField] private Text _enemiesCounter;
        [SerializeField] private Text _waveCounter;
        [SerializeField] private GameObject _loseScreen;

        [Inject]
        public void Initialize(IGameEvents events)
        {
            events.OnWaveLose -= ShowLoseScreen;
            events.OnWaveLose += ShowLoseScreen;
        }
        public void SetEnemiesCounter(int left)
        {
            _enemiesCounter.text = "Врагов осталось: " + left;
        }
        public void SetWaveIndex(int index)
        {
            _waveCounter.text = "Волна номер: " + index;
        }
        public void SetTimer(float value)
        {
            EnableTimer(true);
            _timerBeforeStart.text = "до начала волны осталось: " + value.ToString();
        }
        public void EnableTimer(bool isEnable)
        {
            _timerBeforeStart.enabled = isEnable;
        }

        public void ShowLoseScreen()
        {
            StartCoroutine(LoseRoutine());
        }
        private IEnumerator LoseRoutine()
        {
            yield return new WaitForSeconds(1.2f);
            _loseScreen.SetActive(true);
        }
    }
}
