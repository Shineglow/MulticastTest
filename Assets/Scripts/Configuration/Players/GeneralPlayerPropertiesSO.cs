using UnityEngine;

namespace Characters
{
    [CreateAssetMenu(menuName = "Configuration/GeneralPlayerProperties", fileName = "GeneralPlayerProperties")]
    public class GeneralPlayerPropertiesSO : ScriptableObject
    {
        [field: SerializeField]
        public int MaxAttackedEnemiesAtOnce { get; private set; }
    }
}