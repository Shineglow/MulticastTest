using System.Collections.Generic;
using System.Linq;
using Characters;
using Fusion;
using Network;
using Network.Systems;
using UniRx;
using UnityEngine;
using Zenject;

public class Character : NetworkBehaviour, ICharacter
{
    private const float DEFAULT_ATTACK_SCALE = 2f;
    
    public IProperties Properties => NetworkProperties;
    
    [HideInInspector]
    public NetworkProperties NetworkProperties;
    
    [SerializeField] 
    private DamageTrigger damageTrigger;

    [SerializeField] 
    private Transform attackRadiusView;
    
    private CompositeDisposable _disposables = new CompositeDisposable();
    private EnemiesRegistrar _enemiesRegistrar;

    private void Awake()
    {
        NetworkProperties = GetComponent<NetworkProperties>();
    }

    [Inject]
    public void Initialize(
        [Inject(Id = "initial")]IProperties playerProperties,
        IEnumerable<ILocalPlayerRegister> registers)
    {
        _enemiesRegistrar = new EnemiesRegistrar(damageTrigger, 3);

        if (HasInputAuthority)
        {
            foreach (var register in registers)
            {
                register.RegisterLocalPlayer(this);
            }
        }
        
        if (!HasStateAuthority) return;
        
        NetworkProperties.NetworkRadius = playerProperties.Radius.Value;
        NetworkProperties.NetworkSpeed = playerProperties.Speed.Value;
        NetworkProperties.NetworkDamagePerSecond = playerProperties.DamagePerSecond.Value;
    }

    public override void Spawned()
    {
        base.Spawned();
        
        NetworkProperties.Radius.Subscribe(RadiusUpdated).AddTo(_disposables);
        RadiusUpdated(NetworkProperties.NetworkRadius);
    }

    public override void FixedUpdateNetwork()
    {
        TryInflictDamage();
    }
    
    private void TryInflictDamage()
    {
        var targets = _enemiesRegistrar.GetEnemies(transform.position);

        if (targets == null || targets.Count == 0) return;
        
        var damage = NetworkProperties.DamagePerSecond.Value * Runner.DeltaTime;
        foreach (var target in targets)
        {
            target.DoDamage(damage, this);
        }
    }

    private void RadiusUpdated(float newRadius)
    {
        damageTrigger.Radius = newRadius;
        var a = attackRadiusView.localScale;
        a.x = a.y = DEFAULT_ATTACK_SCALE * damageTrigger.Radius;
        attackRadiusView.localScale = a;
    }
    
    private void OnDestroy()
    {
        _disposables.Dispose();
    }
}
