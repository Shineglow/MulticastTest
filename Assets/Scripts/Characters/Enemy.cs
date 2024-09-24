using System;
using Characters;
using Fusion;
using Network.Network;
using Network.Systems;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Enemy : NetworkBehaviour, INetworkSetActive
{
    public EEnemies EnemyType { get; private set; }
    
    [field: SerializeField][Networked] private float MaxHealth { get; set; } = 100f;
    [field: SerializeField][Networked] private float Health { get; set; } = 100;
    [field: SerializeField] [Networked] private bool activeSoft { get; set; } = true;

    [SerializeField] private ReactiveProperty<float> _reactiveHealth = new(-5);
    [SerializeField] private ReactiveProperty<bool> _alive = new(true);
    public event Action<Enemy, object> Died = (self, killer) => { };

    [SerializeField] private Slider slider;
    [SerializeField] private NetworkCharacterControllerPrototype characterControllerNetwork;
    [SerializeField] private HealthBarAligner healthBarAligner;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Canvas healthBar;

    public bool ActiveSoft => activeSoftReactive.Value;
    private ReactiveProperty<bool> activeSoftReactive = new(true);
    private object _lastHited;

    public override void Spawned()
    {
        base.Spawned();
        
        _alive.Subscribe(newValue =>
        {
            if (!newValue)
            {
                Died?.Invoke(this, _lastHited);
            }
        }).AddTo(this);
        
        _reactiveHealth.Subscribe(newHealth =>
        {
            slider.value = newHealth / MaxHealth;
            _alive.Value = newHealth > 0;
        }).AddTo(this);

        activeSoftReactive.Subscribe(SetActiveSoft).AddTo(this);
        
        MainMonoInstaller.Instance.Inject(this);
    }

    public void SetActiveSoft(bool value)
    {
        activeSoft = value;

        gameObject.layer = value ? LayerMask.NameToLayer("Enemy") : LayerMask.NameToLayer("IgnoreAll");
        meshRenderer.enabled = value;
        healthBar.enabled = value;
    }

    public void Teleport(Vector3 position)
    {
        characterControllerNetwork.Transform.position = position;
    }

    [Inject]
    private void Inject(CameraController cameraController, IKillsCounterSystem killsCounterSystem)
    {
        healthBarAligner.SetCameraTransform(cameraController.transform);
        healthBarAligner.SetTarget(transform);
        killsCounterSystem.Register(this);
    }
    
    public void Initialize(EnemyData data)
    {
        EnemyType = data.Enemy;
        MaxHealth = data.Health;
        Health = data.Health;
    }

    public void FixedUpdate()
    {
        _reactiveHealth.Value = Health;
        activeSoftReactive.Value = activeSoft;
    }

    public void DoDamage(float damage, object damageSource)
    {
        Health = Health < damage ? 0 : Health - damage;
        _lastHited = damageSource;
    }
}
