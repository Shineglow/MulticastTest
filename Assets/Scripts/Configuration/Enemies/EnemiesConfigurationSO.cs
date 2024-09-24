using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    [CreateAssetMenu(menuName = "Configuration/EnemiesConfiguration")]
    public class EnemiesConfigurationSO : ScriptableObject
    {
        [SerializeField]
        private List<EnemyData> _enemies;

        public IReadOnlyList<EnemyData> Enemies => _enemies;
    }
}