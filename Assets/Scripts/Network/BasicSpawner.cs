using System;
using System.Collections.Generic;
using System.Linq;
using Characters;
using Fusion;
using Fusion.Sockets;
using Network.Input;
using Network.Network;
using Network.Systems;
using Network.Systems.Input;
using UnityEngine;
using Zenject;

public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    private NetworkRunner _runner;
    
    private PlayerInputReceiver _playerInputReceiverPrefab;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new();
    private byte _levelsToUp = 0;
    private ISpawnEnemiesSystem _spawnEnemiesSystem;

    [SerializeField]
    private Transform groundTransform;

    private PlayerRef _hostPlayer;
    private INetworkPlayerInput _networkPlayerInput;
    private IEnumerable<INetworkResetable> _resetables;
    private INetworkInputUI _networkInputUI;

    private void Awake()
    {
        _runner = gameObject.AddComponent<NetworkRunner>();
        // _runnerSimulatePhysics3D = gameObject.AddComponent<RunnerSimulatePhysics3D>();
        // _runnerSimulatePhysics3D.ClientPhysicsSimulation = ClientPhysicsSimulation.SimulateForward;
        // _runner.SetSimulateMultiPeerPhysics(true);
        _runner.ProvideInput = true;
    }

    [Inject]
    public void Init(
        INetworkInjecter injecter, 
        PlayerInputReceiver playerInputReceiverPrefab, 
        IMainConfiguration mainConfiguration, 
        ISpawnEnemiesSystem spawnEnemiesSystem,
        INetworkPlayerInput networkPlayerInput,
        INetworkInputUI networkInputUI,
        IEnumerable<INetworkResetable> resetables)
    {
        _playerInputReceiverPrefab = playerInputReceiverPrefab;
        _spawnEnemiesSystem = spawnEnemiesSystem;
        _networkPlayerInput = networkPlayerInput;
        _networkInputUI = networkInputUI;
        _resetables = resetables;
        
        _networkInputUI.Hide();
    }

    private async void StartGame()
    {
        // var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        // var sceneInfo = new NetworkSceneInfo();
        // if (scene.IsValid) {
        //     sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        // }

        NetworkSceneManagerDefault networkSceneManagerDefault = gameObject.AddComponent<NetworkSceneManagerDefault>();

        var result = await _runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.AutoHostOrClient,
            SceneManager = networkSceneManagerDefault,
        });

        if (result.Ok && _runner.IsServer)
        {
            _spawnEnemiesSystem.SpawnEnemies(_hostPlayer, _runner, groundTransform, 50, 5);
        }
    }
    
    private void OnGUI()
    {
        if (_runner == null || !_runner.IsInSession && _runner.State != NetworkRunner.States.Starting)
        {
            if (GUI.Button(new Rect(0,40,200,40), "Join"))
            {
                StartGame();
            }
        }
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.ActivePlayers.Count()) * 3, 1, 0);
            var networkPlayerInputObject = runner.Spawn(_playerInputReceiverPrefab, spawnPosition, Quaternion.identity, player);
            var networkObj = networkPlayerInputObject.GetComponent<NetworkObject>();
            _spawnedCharacters.Add(player, networkObj);
            if (networkPlayerInputObject.HasInputAuthority)
                _hostPlayer = player;
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer && _spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new NetworkInputData();

        data.direction = _networkPlayerInput.CombinedAxes;

        data.levelsToUp = _networkPlayerInput.LevelUpButtonInfo.ClicksCount;

        input.Set(data);
        
        foreach (var resetable in _resetables)
        {
            resetable.OnInputReset();
        }
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner){}
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data){}
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
}
