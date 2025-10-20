using System;

namespace Assets.Scripts.Core.Interfaces
{
    public interface IGameEvents
    {
        public event Action OnWaveWin;
        public event Action OnBonusChanged;
        public event Action OnWaveLose;
    }
}