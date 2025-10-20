using Assets.Scripts.Core.Stats;

namespace Assets.Scripts.Core.Interfaces
{
    public interface IDamageProcessor
    {
        public void InitializeComponents(IEntityHealthBar healthBar, Stat hp);
        void DealDamage(float damage);
    }
}
