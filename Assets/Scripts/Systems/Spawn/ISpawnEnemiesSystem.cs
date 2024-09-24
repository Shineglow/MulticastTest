using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Network.Systems
{
    public interface ISpawnEnemiesSystem
    {
        void SpawnEnemies(PlayerRef playerRef, NetworkRunner runner, Transform ground, int count, float distance,
            int maxIterations = 10);
    }
}