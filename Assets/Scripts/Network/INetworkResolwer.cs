namespace Network.Network
{
    public interface INetworkInjecter
    {
        void Inject<T>(T obj);
    }
}