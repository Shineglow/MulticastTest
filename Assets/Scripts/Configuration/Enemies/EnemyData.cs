using System;
using UnityEngine;

namespace Characters
{
    [Serializable]
    public struct EnemyData
    {
        [field: SerializeField]
        public string Name { get; private set; }
        [field: SerializeField]
        public EEnemies Enemy { get; private set; }
        [field: SerializeField]
        public float Health { get; private set; }
        [field: SerializeField]
        public Enemy Prefab { get; private set; }
    }
}