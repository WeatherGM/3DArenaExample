
using Assets.Scripts.Core.Interfaces;
using System;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Assets.Scripts.Player
{ 
    public class PlayerUpgradeView : MonoBehaviour
    {
        [Header("Buff settings")]
        [SerializeField] private GameObject _screen;
        [SerializeField] private Button _attackBuff;
        [SerializeField] private Button _hpBuff;
        [SerializeField] private Text _attackBuffText;
        [SerializeField] private Text _hpBuffText;
        [Header("Attack indicator")]
        [SerializeField] private Text _attack;


        [Inject]
        public void Initialize(IGameEvents events)
        {
            events.OnWaveWin -= Show;
            events.OnWaveWin += Show;
        }
        public void InitializeActions(Action addAttackHandler, Action addHpHandler)
        {
            _attackBuff.onClick.AddListener(()=> addAttackHandler.Invoke());
            _hpBuff.onClick.AddListener(() => addHpHandler.Invoke());

            _attackBuff.onClick.AddListener(() => _screen.SetActive(false));
            _hpBuff.onClick.AddListener(() => _screen.SetActive(false));
        }
        public void ShowAttack(float attack)
        {
            _attack.text = $"текущий урон {attack}";
        }

        public void InitializeValues(float attack, float hp)
        {
            _attackBuffText.text = $"+{attack} к атаке";
            _hpBuffText.text = $"+{hp} к хп";
        }
        public void Show()
        {
            _screen.SetActive(true);
        }
    }
}
