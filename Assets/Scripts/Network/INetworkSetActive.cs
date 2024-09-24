namespace Network.Network
{
    public interface INetworkSetActive
    {
        bool ActiveSoft { get; }
        void SetActiveSoft(bool value);
    }
}