using Characters.Enums;
using Fusion;
using Network.Extensions;
using UniRx;
using UnityEngine;

namespace Characters
{
    public class NetworkProperties : NetworkBehaviour, IProperties
    {
        public IReadOnlyReactiveProperty<float> Speed => speed;
        public IReadOnlyReactiveProperty<float> DamagePerSecond => damagePerSecond;
        public IReadOnlyReactiveProperty<float> Radius => radius;

        public ReactiveProperty<float> speed;
        public ReactiveProperty<float> damagePerSecond;
        public ReactiveProperty<float> radius;
        
        [Networked] public float NetworkSpeed { get; set; }
        [Networked] public float NetworkDamagePerSecond { get; set; }
        [Networked] public float NetworkRadius { get; set; }

        private void Awake()
        {
            speed = new ReactiveProperty<float>();
            damagePerSecond = new ReactiveProperty<float>();
            radius = new ReactiveProperty<float>();
        }

        public override void Spawned()
        {
            UpdateReactiveFromNetworked();
        }

        private void UpdateReactiveFromNetworked()
        {
            radius.Value = NetworkRadius;
            speed.Value = NetworkSpeed;
            damagePerSecond.Value = NetworkDamagePerSecond;
        }

        public void FixedUpdate()
        {
            UpdateReactiveFromNetworked();
        }
        
        public IReadOnlyReactiveProperty<float> GetCharacterStat(ECharacterStat param) => this.GetReactiveStatByEnum(param);
    }
}