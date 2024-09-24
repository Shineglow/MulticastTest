using Characters.Enums;
using UniRx;

namespace Characters
{
    public interface IProperties
    {
        public IReadOnlyReactiveProperty<float> Speed { get; }
        public IReadOnlyReactiveProperty<float> DamagePerSecond { get; }
        public IReadOnlyReactiveProperty<float> Radius { get; }

        IReadOnlyReactiveProperty<float> GetCharacterStat(ECharacterStat param);
    }
}