using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Stats;
namespace Assets.Scripts.Enemies
{
    public class EnemyDamage : IDamageProcessor
    {
        private IEntityHealthBar _healthBar;
        private Stat _hp;
        public void InitializeComponents(IEntityHealthBar healthBar, Stat hp)
        {
            _healthBar = healthBar;
            _hp = hp;
        }

        public void DealDamage(float damage)
        {
            _hp.Set(_hp.CurrentValue - damage);
            _healthBar.SetHP(_hp.BaseValue, _hp.CurrentValue);
        }
    }   
}
