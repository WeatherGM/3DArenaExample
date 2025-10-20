
using Assets.Scripts.Core.Interfaces;
using System;

namespace Assets.Scripts.Battle
{
    /// <summary>
    /// Отвечает за основные игровые события.
    /// </summary>
    /// <param name="OnBonusChanged">Вызывается, если игрок нажал кнопку бонуса.</param>
    public class BattleEvents : IGameEvents, IGameInvoker
    {
        public event Action OnWaveWin;
        public event Action OnBonusChanged;
        public event Action OnWaveLose;

        public void WaveWin() => OnWaveWin.Invoke();
        public void BonusChanged() => OnBonusChanged.Invoke();
        public void WaveLose() => OnWaveLose.Invoke();
    }
}
