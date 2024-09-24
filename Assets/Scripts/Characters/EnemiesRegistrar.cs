using System.Collections.Generic;
using System.Linq;
using Network;
using UnityEngine;

namespace Characters
{
    public class EnemiesRegistrar
    {
        private HashSet<Enemy> _enemies = new (8);
        private readonly int _maxTargetsToAttack;

        public EnemiesRegistrar(DamageTrigger trigger, int maxTargetsToAttack = 3)
        {
            _maxTargetsToAttack = maxTargetsToAttack;
            trigger.EnemyEnter += OnEnemyEnter;
            trigger.EnemyExit += OnEnemyExit;
        }

        private void OnEnemyExit(Enemy enemy)
        {
            _enemies.Remove(enemy);
            Debug.Log($"Enemy {enemy} removed!");
            enemy.Died -= OnEnemyDied;
        }

        private void OnEnemyEnter(Enemy enemy)
        {
            _enemies.Add(enemy);
            Debug.Log($"Enemy {enemy} added!");
            enemy.Died += OnEnemyDied;
        }

        private void OnEnemyDied(Enemy enemy, object killer)
        {
            _enemies.Remove(enemy);
        }

        public List<Enemy> GetEnemies(Vector3 selfPosition)
        {
            if (_enemies.Count <= 0) return null;

            return _enemies.OrderBy(i => (selfPosition - i.transform.position).sqrMagnitude).Take(_maxTargetsToAttack).ToList();
        }
    }
}