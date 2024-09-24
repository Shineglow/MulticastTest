using System;
using Characters.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace Characters
{
    [Serializable]
    public struct NamedStepAndChance : IStepAndChance
    {
        [FormerlySerializedAs("characterStatName")] [FormerlySerializedAs("parameterName")] [SerializeField] 
        private ECharacterStat eCharacterStatName;
        [SerializeField, Range(0f,PlayerPropertiesProgressionModelSO.MaxChance)] 
        private float chance;
        [SerializeField, Min(0f)] 
        private float step;

        public ECharacterStat ECharacterStatName
        {
            get => eCharacterStatName;
            set => eCharacterStatName = value;
        }

        public float Chance
        {
            get => chance;
            set => chance = value;
        }

        public float Step
        {
            get => step;
            set => step = value;
        }
    }
}