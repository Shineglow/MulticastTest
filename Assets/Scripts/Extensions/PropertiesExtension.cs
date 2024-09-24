using System;
using Characters;
using Characters.Enums;
using UniRx;

namespace Network.Extensions
{
    public static class PropertiesExtension
    {
        public static IReadOnlyReactiveProperty<float> GetReactiveStatByEnum(this IProperties p, ECharacterStat stat)
        {
            return stat switch
            {
                ECharacterStat.Speed => p.Speed,
                ECharacterStat.DamagePerSecond => p.DamagePerSecond,
                ECharacterStat.Radius => p.Radius,
                _ => throw new ArgumentOutOfRangeException(nameof(stat), stat, null)
            };
        }
    }
}