using Unity.VisualScripting;

namespace Assets.Scripts.Core.Interfaces
{
    public interface IGameInvoker
    {
        public void WaveWin();
        public void WaveLose();
        void BonusChanged();
    }
}