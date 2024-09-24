namespace Network.Systems.Input
{
    public interface INetworkPlayerInput : IPlayerInput, INetworkResetable
    {
        ButtonInfoNetwork LevelUpButtonInfo { get; }
    }
}