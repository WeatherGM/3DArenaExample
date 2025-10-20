using System;

namespace Assets.Scripts.Core.Stats
{
    public interface IEntityHealthBar 
    {
        public void SetHP(float @base,float current);
        public void ResetBar();
    }
}