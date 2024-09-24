using System;
using System.Collections.Generic;
using Characters.Enums;
using Fusion;
using Network.Input;
using Network.Systems;
using Network.Systems.Input;
using UniRx;
using UnityEngine;
using Zenject;

namespace Characters
{
    [RequireComponent(typeof(NetworkProperties), typeof(Character))]
    public class PlayerInputReceiver : NetworkBehaviour
    {
        private NetworkProperties _networkProperties;
        private Character _character;
        public ICharacter Character => _character;
        
        private NetworkCharacterControllerPrototype _cc;
        
        private LevelingSystem _levelingSystem;
        
        private CompositeDisposable _disposables = new CompositeDisposable();

        public override void Spawned()
        {
            _networkProperties = GetComponent<NetworkProperties>();
            _cc = GetComponent<NetworkCharacterControllerPrototype>();
            _character = GetComponent<Character>();
            
            _networkProperties.Speed.Subscribe(speed => _cc.maxSpeed = speed).AddTo(_disposables);

            MainMonoInstaller.Instance.Inject(_character);
            MainMonoInstaller.Instance.Inject(this);
        }

        [Inject]
        private void Inject(
            CharacterStatTableView characterStatView, 
            LevelingSystem levelingSystem, 
            CameraController cameraController, 
            INetworkInputUI networkInputUI)
        {
            _levelingSystem = levelingSystem;

            if (HasInputAuthority)
            {
                networkInputUI.Show();
                
                cameraController.FollowCharacter(transform);

                var capacity = Enum.GetValues(typeof(ECharacterStat)).Length;
                List<(string name, IReadOnlyReactiveProperty<float> value)> data = new (capacity);
            
                data.Add((ECharacterStat.Radius.ToString(), _networkProperties.Radius));
                data.Add((ECharacterStat.DamagePerSecond.ToString(), _networkProperties.DamagePerSecond));
                data.Add((ECharacterStat.Speed.ToString(), _networkProperties.Speed));
            
                characterStatView.Init(data);
            }
        }

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out NetworkInputData data))
            {
                data.direction.Normalize();
                _cc.Move(_networkProperties.Speed.Value * data.direction * Runner.DeltaTime);

                if (HasStateAuthority && data.levelsToUp > 0)
                {
                    _levelingSystem.UpgradeCharacterStat(_character, data.levelsToUp);
                }
            }
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}