using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Characters
{
    [CreateAssetMenu(menuName = "Configuration/PlayerPropertiesProgressionModel", fileName = "PlayerPropertiesProgressionModel")]
    public class PlayerPropertiesProgressionModelSO : ScriptableObject
    {
        public const float MaxChance = 100f;
        
        [SerializeField] 
        private List<NamedStepAndChance> models = new();

        public IReadOnlyList<NamedStepAndChance> Models => models;
        
        public void NormalizeChanges()
        {
            float sum = models.Sum(i => i.Chance);
            
            if (sum == 0f)
            {
                models.ForEach(model =>
                {
                    model.Chance = MaxChance / models.Count;
                });
            }
            else
            {
                models.ForEach(model =>
                {
                    model.Chance = model.Chance / sum * MaxChance;
                });
            }
        }
    }
}