using Characters;
using Fusion;
using Network.Network;
using Network.Systems;
using Network.Systems.Input;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class MainMonoInstaller : MonoInstaller, INetworkInjecter
{
    public static MainMonoInstaller Instance;
    [SerializeField] 
    private MainConfigurationSO mainConfigurationSo;

    [SerializeField] 
    private BasicSpawner basicSpawnerPrefab;

    [SerializeField] 
    private CharacterStatTableView characterStatTableView;

    [SerializeField] 
    private CameraController cameraController;
    
    [SerializeField]
    private PlayerInputReceiver playerInputReceiverPrefab;
    [SerializeField]
    private InputUI _inputUI;

    [SerializeField] 
    private EnemiesDiedView enemiesDiedView;

    public override void InstallBindings()
    {
        Container.BindInterfacesTo<MainMonoInstaller>().FromInstance(this).AsCached();
        Container.BindInterfacesTo<MainConfigurationSO>().FromInstance(mainConfigurationSo).AsCached();
        Container.Bind<LevelingSystem>().AsCached();
        Container.Bind<PlayerPropertiesProgressionModelSO>()
            .FromInstance(mainConfigurationSo.PlayerPropertiesProgressionModelSo).AsCached();
        Container.Bind<EnemiesConfigurationSO>()
            .FromInstance(mainConfigurationSo.EnemiesConfiguration).AsCached();
        Container.Bind<GeneralPlayerPropertiesSO>()
            .FromInstance(mainConfigurationSo.GeneralPlayerProperties).AsCached();
        Container.Bind<IProperties>().WithId("initial")
            .FromInstance(mainConfigurationSo.InitialPlayerProperties).AsCached();
        Container.Bind<PlayerInputReceiver>().FromInstance(playerInputReceiverPrefab);
        Container.Bind<CameraController>().FromInstance(cameraController).AsCached();
        Container.Bind<CharacterStatTableView>().FromInstance(characterStatTableView).AsCached();
        Container.BindInterfacesTo<INetworkRunnerCallbacks>().FromInstance(basicSpawnerPrefab);
        Container.BindInterfacesTo<SpawnEnemiesSystem>().FromNew().AsCached();
        Container.Bind<EnemiesDiedView>().FromInstance(enemiesDiedView).AsCached();
        Container.BindInterfacesTo<KillsCounterSystem>().FromNew().AsCached();

        Container.BindInterfacesTo<InputUI>().FromInstance(_inputUI).AsCached();
        Container.BindInterfacesTo<PlayerInput>().FromNew().AsCached();

        Instance = this;
        
        Container.Inject(basicSpawnerPrefab);
    }

    public void Inject<T>(T obj)
    {
        Container.Inject(obj);
    }
}