using System;
using Characters;
using Characters.Enums;

namespace Network.Systems
{
    public class LevelingSystem : ILevelingSystem
    {
        private readonly PlayerPropertiesProgressionModelSO _progressionModelSo;
        
        public LevelingSystem(PlayerPropertiesProgressionModelSO progressionModelSo)
        {
            _progressionModelSo = progressionModelSo;
        }
        public void UpgradeCharacterStat(Character character, int levelsToUp)
        {
            if (!character.HasStateAuthority || levelsToUp <= 0) return;
            Properties p = new Properties();
            
            while (levelsToUp-- > 0)
            {
                var chance = UnityEngine.Random.Range(0, PlayerPropertiesProgressionModelSO.MaxChance);
                float chanceSum = 0;
                foreach (var namedStepAndChance in _progressionModelSo.Models)
                {
                    chanceSum += namedStepAndChance.Chance;
                    if (!(chance <= chanceSum)) continue;
                    
                    switch (namedStepAndChance.ECharacterStatName)
                    {
                        case ECharacterStat.Speed:
                            p.Speed += namedStepAndChance.Step;
                            break;
                        case ECharacterStat.DamagePerSecond:
                            p.DamagePerSecond += namedStepAndChance.Step;
                            break;
                        case ECharacterStat.Radius:
                            p.Radius += namedStepAndChance.Step;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                }
            }
            
            character.NetworkProperties.NetworkRadius += p.Radius;
            character.NetworkProperties.NetworkDamagePerSecond += p.DamagePerSecond;
            character.NetworkProperties.NetworkSpeed += p.Speed;
        }
    }
}