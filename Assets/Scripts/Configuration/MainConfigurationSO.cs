using UnityEngine;

namespace Characters
{
    [CreateAssetMenu(menuName = "Configuration/MainConfiguration", fileName = "MainConfiguration")]
    public class MainConfigurationSO : ScriptableObject, IMainConfiguration
    {
        public IProperties InitialPlayerProperties => Properties;

        [field: SerializeField]
        public PlayerPropertiesProgressionModelSO PlayerPropertiesProgressionModelSo { get; private set; }

        [SerializeField]
        private PropertiesSO Properties;
        
        [field: SerializeField]
        public GeneralPlayerPropertiesSO GeneralPlayerProperties { get; private set; }

        [field: SerializeField] 
        public EnemiesConfigurationSO EnemiesConfiguration;
    }
}