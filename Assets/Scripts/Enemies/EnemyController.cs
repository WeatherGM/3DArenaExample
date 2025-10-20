using Assets.Scripts.Core.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;
namespace Assets.Scripts.Core.Enemies
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        private HashSet<EnemyEntity> _controlledEnemies = new();
        
        public void AddEnemy(EnemyEntity enemy)
        {
            _controlledEnemies.Add(enemy);
        }
        public void KillAll()
        {
            foreach (var enemy in _controlledEnemies)
                enemy.EndLiving();
        }
        void Update()
        {
            for (int i = 0; i < _controlledEnemies.Count; i++)
            {
                var enemy = _controlledEnemies.ElementAt(i);


                if (enemy.IsLive == false)
                {
                    _controlledEnemies.Remove(enemy);
                    return;
                }

                if (enemy.NearTarget)
                    enemy.Attack();

                var lookVector = new Vector3(_target.position.x, enemy.transform.position.y, _target.position.z);
                enemy.transform.LookAt(lookVector);

                enemy.Move(_target.position);
                
            }
        }
    }
}