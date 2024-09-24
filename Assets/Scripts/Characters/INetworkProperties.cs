namespace Characters
{
    public interface INetworkProperties
    {
        float NetworkSpeed { get; set; }
        float NetworkDamagePerSecond { get; set; }
        float NetworkRadius { get; set; }
    }
}