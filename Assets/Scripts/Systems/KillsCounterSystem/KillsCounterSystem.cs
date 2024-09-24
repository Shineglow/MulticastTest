using Characters;

namespace Network.Systems
{
    public class KillsCounterSystem : IKillsCounterSystem, ILocalPlayerRegister
    {
        private readonly EnemiesDiedView _view;
        private object _localPlayer;
        private int Kills { get; set; }

        public KillsCounterSystem(EnemiesDiedView view)
        {
            _view = view;
        }

        public void Register(Enemy enemy)
        {
            enemy.Died += UpdateKills;
        }

        private void UpdateKills(Enemy obj, object killer)
        {
            if (_localPlayer == null || !ReferenceEquals(killer, _localPlayer)) return;
            Kills++;
            _view.SetDiedEnemiesCount(Kills);
        }

        public void RegisterLocalPlayer(ICharacter player)
        {
            _localPlayer = player;
        }
    }
}