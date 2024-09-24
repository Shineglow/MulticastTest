using Characters.Enums;
using Network.Extensions;
using UniRx;
using UnityEngine;
using Zenject;

namespace Characters
{
    [CreateAssetMenu(menuName = "Configuration/PropertiesSO", fileName = "PropertiesSO")]
    public class PropertiesSO : ScriptableObject, IProperties
    {
        public IReadOnlyReactiveProperty<float> Speed => speed;
        public IReadOnlyReactiveProperty<float> DamagePerSecond => damagePerSecond;
        public IReadOnlyReactiveProperty<float> Radius => radius;
        
        [SerializeField] public ReactiveProperty<float> speed;
        [SerializeField] public ReactiveProperty<float> damagePerSecond;
        [SerializeField] public ReactiveProperty<float> radius;

        [field: SerializeField][Inject]
        public GeneralPlayerPropertiesSO GeneralPlayerProperties { get; private set; }

        public IReadOnlyReactiveProperty<float> GetCharacterStat(ECharacterStat param) => this.GetReactiveStatByEnum(param);
    }
}