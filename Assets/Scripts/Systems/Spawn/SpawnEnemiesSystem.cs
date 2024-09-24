using System;
using System.Collections.Generic;
using Characters;
using Fusion;
using Network.Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Network.Systems
{
    public class SpawnEnemiesSystem : ISpawnEnemiesSystem
    {
        private readonly EnemiesConfigurationSO _enemiesConfiguration;
        
        private readonly List<Enemy> enemies = new (30);
        
        private Dictionary<EEnemies, List<Enemy>> objectsPool = new ();
        private Vector3 _lowerLeftCorner;
        private Vector3 _max;
        private NetworkRunner _runner;

        public SpawnEnemiesSystem(EnemiesConfigurationSO enemiesConfiguration)
        {
            _enemiesConfiguration = enemiesConfiguration;

            foreach (EEnemies enemy in Enum.GetValues(typeof(EEnemies)))
            {
                objectsPool.Add(enemy, new());
            }
        }

        public void SpawnEnemies(PlayerRef playerRef, NetworkRunner runner, Transform ground, int count, float distance, int maxIterations = 10)
        {
            if (!runner.IsServer) return;

            _runner = runner;
            
            // Setup some data
            // some of the data is not contained in the configuration and is not transmitted externally
            // as it should be if I had more time.
            var size = ground.localScale;
            var offsetUp = Vector3.up * size.y / 2;
            var surfaceCenterPoint = ground.position + offsetUp;
            var workSpace = new Vector3(size.x - 1, 0, size.z - 1);
            _lowerLeftCorner = surfaceCenterPoint - workSpace / 2;
            _max = _lowerLeftCorner + workSpace;

            for (var i = 0; i < count; i++)
            {
                var enemyData = _enemiesConfiguration.Enemies.GetRandom();
                var randomPos = GetRandomVector3(_lowerLeftCorner, _max, Vector3.zero, 5f);
                InstantiateEnemy(enemyData, randomPos, playerRef);
            }
        }

        private void Respawn(Enemy enemy, object killer)
        {
            var enemyData = _enemiesConfiguration.Enemies.GetRandom();
            var randomPos = GetRandomVector3(_lowerLeftCorner, _max, Vector3.zero, 5f);
            
            // if new enemy same type as died - reinitialize
            if (enemy.EnemyType == enemyData.Enemy)
            {
                ResetEnemy(enemy);
                return;
            }

            // if have inactive enemy of same type - swap to faster finding inactive next time
            enemy.SetActiveSoft(false);
            var poolByType = objectsPool[enemy.EnemyType];
            
            var firstActive = poolByType.FindIndex(i => i.ActiveSoft);
            if (firstActive >= 0)
            {
                var died = poolByType.IndexOf(enemy);
                (poolByType[firstActive], poolByType[died]) = (poolByType[died], poolByType[firstActive]);
            }

            // if need another enemy, try to find inactive, else - spawn
            var freeEnemy = objectsPool[enemyData.Enemy].Find(i => !i.ActiveSoft);
            if (freeEnemy == null)
            {
                InstantiateEnemy(enemyData, randomPos);
            }
            else
            {
                ResetEnemy(freeEnemy);
            }

            void ResetEnemy(Enemy localEnemy)
            {
                localEnemy.SetActiveSoft(true);
                localEnemy.Teleport(randomPos);
                localEnemy.Initialize(enemyData);
            }
        }

        private void InstantiateEnemy(EnemyData enemyData, Vector3 randomPos, PlayerRef playerRef = default)
        {
            if (!_runner.IsServer) return;
            
            var enemy = _runner.Spawn(enemyData.Prefab, randomPos, Quaternion.identity, playerRef);
            enemy.Initialize(enemyData);
            enemy.Died += Respawn;
            objectsPool[enemyData.Enemy].Add(enemy);
        }

        private Vector3 GetRandomVector3(Vector3 min, Vector3 max, Vector3 excludeFrom = default, float excludeDistance = 0f)
        {
            Vector3 rnd = new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
            
            if (excludeDistance == 0)
                return rnd;
            
            var direction = rnd - excludeFrom;
            var magnitude = direction.magnitude;

            if (magnitude < excludeDistance)
            {
                var magnitudeDif = magnitude - excludeDistance;
                rnd += direction.normalized * magnitudeDif;
            }
            
            return rnd;
        }
    }
}