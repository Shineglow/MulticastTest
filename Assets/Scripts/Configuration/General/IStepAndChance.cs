using Characters.Enums;

namespace Characters
{
    public interface IStepAndChance
    {
        ECharacterStat ECharacterStatName { get; }
        float Chance { get; }
        float Step { get; }
    }
}